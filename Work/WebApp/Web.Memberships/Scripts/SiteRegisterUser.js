var registerUser = {
    //these teo will be filled in at run time from the Home/Index View
    _urls: {
        _registerUser: '', // /Account/RegisterUserAsync
        _siteHome: '', // /Home/Index
    },
    _selectors: {
        formId: '#SiteRegisterUserPartial',
        acceptUserAgreementId: '#AcceptUserAgreement',
        buttonSelector: '.register-user-panel button',
        antiForgeryTokenSelector: '[name="__RequestVerificationToken"]',
        firstNameSelector: '.register-user-panel .first-name',
        lastNameSelector: '.register-user-panel .last-name',
        emailSelector: '.register-user-panel .email',
        pwdSelector: '.register-user-panel .password',
        confirmPwdSelector: '.register-user-panel .confirm-password',
        validationSummarySelector: '[data-valmsg-summary]',
        registerUserPanelSelector: '.register-user-panel'
    },
    _bootstrapClasses: {
        disabled: 'disabled'
    },
    //receives an object with names siteHomeUrl, registerUserUrl
    init: function (arg) {
        //we are not using the registerUserUrl for the implemented solution
        //it is being used by the commended code in onRegisterUserClick function
        registerUser._urls._siteHome = arg.siteHomeUrl;
        registerUser._urls._registerUser = arg.registerUserUrl;

        //if the checkbox is checked then uncheck it
        var $acceptUserAgreement = $(registerUser._selectors.acceptUserAgreementId);
        if ($acceptUserAgreement.is(":checked")) {
            $acceptUserAgreement.prop("checked", false);
        }

        //wireup the clicks
        //registerUser.wireUpClicks();
    },
    wireUpClicks: function () {
        //wire up the clicks

        $(registerUser._selectors.acceptUserAgreementId).click(function (e) {
            registerUser.onToggleRegisterUserDisabledClick();
        });

        $(registerUser._selectors.buttonSelector).click(function (e) {
            //call the onRegisterUserClick method
            registerUser.onRegisterUserClick()
            e.preventDefault();
        });
    },
    onToggleRegisterUserDisabledClick: function () {
        $(registerUser._selectors.buttonSelector).toggleClass(registerUser._bootstrapClasses.disabled);
    },
    onRegisterUserClick: function () {
        var $button = $(registerUser._selectors.buttonSelector);
        if ($button.hasClass("disabled")) {
            return;
        }

        var $form = $(registerUser._selectors.formId);
        if ($form.length <= 0) {
            alert("Unable to get form information!");
            return;
        }

        //check form is valid
        if (!$form.valid()) {
            return;
        }

        //display spinner
        siteSpinner.addSpinner($button);
        registerUser.onToggleRegisterUserDisabledClick();

        $.ajax({
            cache: false,
            url: $form.attr("action"),
            type: $form.attr("method"),
            data: $form.serialize(), 
            error: function (xhr, ajaxOptions, thrownError) {
                siteSpinner.removeSpinner($button);
                registerUser.onToggleRegisterUserDisabledClick();

                alert(xhr.responseText);
            },
            success: function (data) {
                siteSpinner.removeSpinner($button);
                registerUser.onToggleRegisterUserDisabledClick();

                var parsed = $.parseHTML(data);
                var hasErrors = $(parsed).find(registerUser._selectors.validationSummarySelector).text().replace(/\n|\r/g, "").length > 0;
                if (hasErrors) {
                    //error => display the data back 
                    $(registerUser._selectors.registerUserPanelSelector).html(data);
                    $(registerUser._selectors.buttonSelector).removeClass(registerUser._bootstrapClasses.disabled);
                    //validation is not bound on the dynamically rendered form
                    $.validator.unobtrusive.parse(document);
                }
                else {
                    //redirect
                    location.href = registerUser._urls._siteHome;
                }
            }
        });

        /*
        var antiforgery = $(registerUser._selectors.antiForgeryTokenSelector).val();
        var firstName = $(registerUser._selectors.firstNameSelector).val();
        var lastName = $(registerUser._selectors.lastNameSelector).val();
        var email = $(registerUser._selectors.emailSelector).val();
        var pwd = $(registerUser._selectors.pwdSelector).val();
        var pwdConfirm = $(registerUser._selectors.confirmPwdSelector).val();

        $.post(url, { __RequestVerificationToken: antiforgery, email: email, firstName: firstName, lastName: lastName, password: pwd, confirmPassword: pwdConfirm, acceptUserAgreement: true },
            function (data) {
                var parsed = $.parseHTML(data);
                var hasErrors = $(parsed).find(registerUser._selectors.validationSummarySelector).text().replace(/\n|\r/g, "").length > 0;

                if (hasErrors) {
                    $(registerUser._selectors.registerUserPanelSelector).html(data);
                    registerUser.onToggleRegisterUserDisabledClick();
                    registerUser.onRegisterUserClick();
                    $(registerUser._selectors.buttonSelector).removeClass('disabled');
                }
                else {
                    //registerUser.onToggleRegisterUserDisabledClick();
                    //registerUser.onRegisterUserClick();
                    location.href = registerUser._urls._siteHome;
                }
            })
            .fail(function (xhr, status, error) {
                alert('Post unsuccessful');
            });
        */
    }
};
$(function (e) {
    //we'll do the wireup on the body incase of an error the partial view is returned back
    /*
    $(registerUser._selectors.acceptUserAgreementId).click(function (e) {
        registerUser.onToggleRegisterUserDisabledClick();
    });

    $(registerUser._selectors.buttonSelector).click(function (e) {
        //call the onRegisterUserClick method
        registerUser.onRegisterUserClick()
        e.preventDefault();
    });
    */

    $('body').on('click', registerUser._selectors.acceptUserAgreementId, function (e) {
        registerUser.onToggleRegisterUserDisabledClick();
    });

    $('body').on('click', registerUser._selectors.buttonSelector, function (e) {
        //call the onRegisterUserClick method
        registerUser.onRegisterUserClick()
        e.preventDefault();
    });

});