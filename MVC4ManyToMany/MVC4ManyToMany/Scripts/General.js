!function ($) {
    var publicMethods = ["hide", "show"];
    var Spinner = function ($target) {
        $target = $target instanceof jQuery ? $target : $($target);
        $target.css("position") == "absolute" ? null :
        $target.css("position", "relative");
        this.$spinner = $("<div class='spinner hidden pink-spinner'>");
        $target.append(this.$spinner)
    };
    Spinner.prototype = {
        constructor: Spinner,
        show: function () {
            this.$spinner.removeClass("hidden")
        },
        hide: function () {
            this.$spinner.addClass("hidden")
        }
    };
    $.fn.spinner = function () {
        return this.each(function () {
            var $this = $(this),
                data = $this.data("spinner");
            if (data === undefined) {
                $this.data("spinner", (data = new Spinner($this)));
                $this.bind("spinner:show", function (e) {
                    if (e.currentTarget !== e.target) {
                        return
                    }
                    data.show.call(data)
                }).bind("spinner:hide", function (e) {
                    if (e.currentTarget !== e.target) {
                        return
                    }
                    data.hide.call(data)
                })
            }
        })
    };
    $.fn.spinner.Constructor = Spinner
}(window.jQuery);
