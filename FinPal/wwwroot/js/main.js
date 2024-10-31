(function () {
    /**
       * Mobile nav toggle
       */
    function on(eventType, selector, callback) {
        document.addEventListener(eventType, function (e) {
            if (e.target.matches(selector)) {
                callback.call(e.target, e);
            }
        });
    }
})();