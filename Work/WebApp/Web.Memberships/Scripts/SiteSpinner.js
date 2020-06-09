/*
 must have a child span
 <button type="button" class="pull-right btn btn-success disabled">
    <span class="spinner"></span>
        Sign me up!
</button>
*/
var siteSpinner = {
    _enumSpinner: {
        selector: '.spinner',
        classes: 'glyphicon glyphicon-refresh glyphicon-refresh-animate'
    },
    _isObject: function ($element) {
        if ($element === null || $element === undefined)
            return false;
        if (!$element.jquery)
            return false;
        return $element.has(siteSpinner._enumSpinner.selector) !== 0;
    },
    addSpinner: function ($element) {
        if (!siteSpinner._isObject($element))
            return;
        $element.find(siteSpinner._enumSpinner.selector).addClass(siteSpinner._enumSpinner.classes);
    },
    removeSpinner: function($element) {
        if (!siteSpinner._isObject($element))
            return;
        $element.find(siteSpinner._enumSpinner.selector).removeClass(siteSpinner._enumSpinner.classes);
    }
};