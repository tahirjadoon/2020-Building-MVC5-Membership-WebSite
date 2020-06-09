//to keep the code dry, using the vars where possible
var registerCode = {
    //we can hard code here or better way is to fill from the home\index view where the partial view is displaying
    _registerCodeUrl: '', //In case to hard code /RegisterCode/Register
    //different selectors
    _codeInputSelector: '.register-code-panel input', 
    _alertDivSelector: '.register-code-panel .alert', 
    _buttonSelector: '.register-code-panel button',
    //bootstrap classes
    _alertClass: {
        danger: 'alert-danger', 
        success: 'alert-success', 
        hidden: 'hidden'
    },
    //function to display the message
    displayMessage: function (success, message) {
        var $alertDiv = $(registerCode._alertDivSelector);
        $alertDiv.text(message);

        //defaults
        var addClass = registerCode._alertClass.success;
        var removeClass = registerCode._alertClass.danger;
        if (!success) {
            addClass = registerCode._alertClass.danger;
            removeClass = registerCode._alertClass.success;
        }

        //hidden class is added by the register function below
        $alertDiv.removeClass(removeClass).addClass(addClass).removeClass(registerCode._alertClass.hidden);
    }, 
    //function to register
    register: function () {
        //jquery objects
        var $alert = $(registerCode._alertDivSelector);
        var $codeInput = $(registerCode._codeInputSelector);
        //hide the alert div
        $alert.addClass(registerCode._alertClass.hidden);
        //check have url
        if (registerCode._registerCodeUrl === "" || registerCode._registerCodeUrl.length <= 0) {
            registerCode.displayMessage(false, "Could not register. \n\rConnect with the site owner to resolve the issue!");
            return;
        }
        //code val
        var code = $codeInput.val();
        if (code.length <= 0) {
            registerCode.displayMessage(false, "Enter a Code");
            return;
        }

        var $button = $(registerCode._buttonSelector);
        //add spinner
        siteSpinner.addSpinner($button);
        //post
        $.post(registerCode._registerCodeUrl, { code: code },
            function (data) {
                //remove spinner
                siteSpinner.removeSpinner($button);
                registerCode.displayMessage(true, "The code was successfully added. \n\rPlease reload the page.");
                $codeInput.val('');
            }).fail(function (xlr, status, error) {
                //remove spinner
                siteSpinner.removeSpinner($button);
                registerCode.displayMessage(false, "Could not register the code");
                });

    }

};

//click on the button
$(function (e) {
    $(registerCode._buttonSelector).click(function (e) {
        //call the register method
        registerCode.register();
        e.preventDefault();
    });
});