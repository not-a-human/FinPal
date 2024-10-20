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
    on('click', '.mobile-nav-toggle', function (e) {
        document.querySelector('body').classList.toggle('mobile-nav-active');
        this.classList.toggle('bi-list');
        this.classList.toggle('bi-x');
    });
})();