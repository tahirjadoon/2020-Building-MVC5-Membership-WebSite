var siteLogin = {
    _selectors: {
        loginLinkId: '#loginLink',
        closeLoginId: '#close-login',
        loginButtonId: '#login-button',
        formId: '#SiteLoginPartial', 
        validationSummarySelector: '[data-valmsg-summary]'
    },
    _attributes: {
        loginButtonReturnUrl: 'data-login-return-url',
        userArea: 'div[data-login-user-area]',
        loginPanelPartial: 'div[login-panel-partial]' //in _LoginPartial.cshtml
    },
    _bootstrapClasses: {
        disabled: 'disabled'
    },
    onLoginLinkHover: function () {
        $(siteLogin._attributes.userArea).addClass('open');
    },
    onCloseLogin: function () {
        $(siteLogin._attributes.userArea).removeClass('open');
    },
    onTogglLoginButtonDisabledClick: function ($button) {
        $button.toggleClass(siteLogin._bootstrapClasses.disabled);
    },
    onFormSubmit: function ($form, myform) {
        var $loginButton = $(siteLogin._selectors.loginButtonId);
        //a request is executing
        if ($loginButton.hasClass(siteLogin._bootstrapClasses.disabled)) return;
        //check form is valid
        if (!$form.valid()) {
            return;
        }
        //disable button
        siteLogin.onTogglLoginButtonDisabledClick($loginButton);
        //pick return url 
        var returnUrl = $loginButton.attr(siteLogin._attributes.loginButtonReturnUrl);
        if (returnUrl === null || returnUrl === undefined || returnUrl === '')
            returnUrl = '/';

        //add spinner
        siteSpinner.addSpinner($loginButton);

        //submit form 
        $.ajax({
            cache: false,
            url: $form.attr("action"),
            type: $form.attr("method"),
            data: $form.serialize(),
            error: function (xhr, ajaxOptions, thrownError) {
                siteSpinner.removeSpinner($loginButton);
                siteLogin.onTogglLoginButtonDisabledClick($loginButton);

                alert(xhr.responseText);
            },
            success: function (data) {
                siteSpinner.removeSpinner($loginButton);
                siteLogin.onTogglLoginButtonDisabledClick($loginButton);

                var parsed = $.parseHTML(data);
                var hasErrors = $(parsed).find(siteLogin._selectors.validationSummarySelector).text().replace(/\n|\r/g, "").length > 0;
                if (hasErrors) {
                    //error => display the data back 
                    $(siteLogin._attributes.loginPanelPartial).html(data);
                    //validation is not bound on the dynamically rendered form
                    $.validator.unobtrusive.parse(document);
                }
                else {
                    //redirect
                    location.href = returnUrl;
                }
            }
        });
        
    }
};



$(function () {
    //we'll do the wireup on the body incase of an error the partial view is returned back

    //on hover over login open the login panel
    $('body').on('mouseover', siteLogin._selectors.loginLinkId, function (e) {
        siteLogin.onLoginLinkHover();
    });

    //on close button click close the login panel
    $('body').on('click', siteLogin._selectors.closeLoginId, function (e) {
        siteLogin.onCloseLogin();
    });

    //form submit
    $('body').on('submit', siteLogin._selectors.formId, function (e) {
        e.preventDefault();
        siteLogin.onFormSubmit($(this), this);
    });
});


 /*
    var loginLinkHover = $("#loginLink").hover(onLoginLinkHover);
    var loginCloseButton = $("#close-login").click(onCloseLogin);
    var loginButton = $("#login-button").click(onLoginClick);

    function onLoginLinkHover() {
        $("div[data-login-user-area]").addClass('open');
    }

    function onCloseLogin() {
        $("div[data-login-user-area]").removeClass('open');
    }

    function onLoginClick() {
        var url = $('#login-button').attr('data-login-action');
        var return_url = $('#login-button').attr('data-login-return-url');
        var email = $('#Email').val();
        var pwd = $('#Password').val();
        var remember_me = $('#RememberMe').prop('checked');
        var antiforgery = $('[name="__RequestVerificationToken"]').val();

        $.post(url, {
            __RequestVerificationToken: antiforgery, email: email,
            password: pwd, RememberMe: remember_me
        }, function (data) {
            var parsed = $.parseHTML(data);
            var hasErrors = $(parsed).find("[data-valmsg-summary]").text()
                .replace(/\n|\r/g, "").length > 0;

            if (hasErrors == true) {
                alert(1);
                $('div[data-login-panel-partial]').html(data);
                $('div[data-login-user-area]').addClass('open');
                $('#Email').addClass('data-login-error');
                $('#Password').addClass('data-login-error');
            }
            else {
                alert(2);
                $('div[data-login-user-area]').removeClass('open');
                $('#Email').removeClass('data-login-error');
                $('#Password').removeClass('data-login-error');
                location.href = return_url;
            }
            loginLinkHover = $("#loginLink").hover(onLoginLinkHover);
            loginCloseButton = $("#close-login").click(onCloseLogin);
            loginButton = $("#login-button").click(onLoginClick);
        }).fail(function (xhr, status, error) {
            alert(3);
            $('#Email').addClass('data-login-error');
            $('#Password').addClass('data-login-error');

            loginLinkHover = $("#loginLink").hover(onLoginLinkHover);
            loginCloseButton = $("#close-login").click(onCloseLogin);
            loginButton = $("#login-button").click(onLoginClick);
        });
    }
    */