Cart = {
    _properties: {
        getCartViewLink: "",
        addToCartLink: "",
        decrementLink: "",
        removeFromCartLink: ""
    },

    init: function(properties) {
        $.extend(Cart._properties, properties);

        $(".add-to-cart").click(Cart.addToCart);
    },

    addToCart: function(event) {
        event.preventDefault();

        var button = $(this);
        const id = button.data("id");

        $.get(Cart._properties.addToCartLink + "/" + id)
            .done(function(response) {
                Cart.showToolTip(button);
                Cart.refreshCartView();

                console.log(response.message);
            })
            .fail(function () { console.log("addToCart fail"); });
    },

    showToolTip: function(button) {
        button.tooltip({ title: "Добавлено в корзину" }).tooltip("show");
        setTimeout(function() {
                button.tooltip("destroy");
            },
            500);
    },

    refreshCartView: function() {
        $.get(Cart._properties.getCartViewLink)
            .done(function(cartHtml) {
                $("#cart-container").html(cartHtml);
            })
            .fail(function () { console.log("refreshCartView fail"); })
    }
}