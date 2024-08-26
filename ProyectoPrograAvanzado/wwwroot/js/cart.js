$(document).ready(function () {
    const cart = JSON.parse(localStorage.getItem('cart')) || [];
    function updateCartUI() {
        $('#cart-items').empty();
        let total = 0;
        if (cart.length === 0) {
            $('#cart-message').show();
            $('#checkout-section').hide();
            $('#clear-cart').hide();
        } else {
            $('#cart-message').hide();
            $('#checkout-section').show();
            $('#clear-cart').show();
            cart.forEach((item, index) => {
                $('#cart-items').append(`
                    <li class="list-group-item d-flex justify-content-between align-items-center">
                        ${item.name} - $${item.price.toFixed(2)}
                        <button class="btn btn-danger btn-sm remove-item" data-index="${index}">Remove</button>
                    </li>
                `);
                total += item.price;
            });
        }
        $('#cart-total').text(`Total: $${total.toFixed(2)}`);
    }
    function addToCart(productId, productName, productPrice, productDetails) {
        cart.push({ id: productId, name: productName, price: productPrice, productDetails: productDetails });
        localStorage.setItem('cart', JSON.stringify(cart));
        updateCartUI();
    }
    $('body').on('click', 'button[id^="add-to-cart-"]', function () {
        const productId = $(this).attr('id').split('-').pop();
        const productName = $(`#product-name-${productId}`).text();
        const productPriceText = $(`#product-price-${productId}`).text();
        const productPrice = parseFloat(productPriceText.replace(/[^0-9.-]/g, ''));
        const productDetails = $(`#product-details-${productId}`).text();
        if (!isNaN(productPrice)) {
            addToCart(productId, productName, productPrice, productDetails);
        } else {
            console.error('Invalid price format');
        }
    });
    $('body').on('click', 'button.remove-item', function () {
        const index = $(this).data('index');
        if (index > -1) {
            cart.splice(index, 1);
            localStorage.setItem('cart', JSON.stringify(cart));
            updateCartUI();
        }
    });
    $('body').on('click', '#clear-cart', function () {
        cart.length = 0;
        localStorage.removeItem('cart');
        updateCartUI();
    });
    updateCartUI();
});