        // Show modal add to cart 

$(document).ready(function () {

    // Click on button add to cart

    $('.btn-add-to-cart').click(function () {
        var element = document.getElementById("addToCart");
        element.classList.remove('hide');
        element.classList.add('show-modal');
        console.log("click count");

        // Lay ve product id
        // Ajax hien thi san pham tuong ung vao modal
        var productId = $(this).attr('data-id');
        console.log(productId);

        $.ajax({
            type: 'POST',
            async: false,
            data: {
                product_id: productId
            },
            url: '/customer/product/findProductById',
            success: function (product) {
                console.log(product);
                $('.product-name').text(product.name);
                $('.product-id').text('Mã sản phẩm: '+product.producT_ID);
                $('#product-img').attr('src', './admin/images/products/' + product.image);
                var priceFormatted = product.price.toLocaleString('it-IT', { style: 'currency', currency: 'VND' });
                $('.product-price').text(priceFormatted.replace('.',','));
            }
        });

        $.ajax({
            type: 'POST',
            async: false,
            data: {
                product_id: productId
            },
            url: '/customer/product/findAllSizeOfProducts',
            success: function (data) {
                var result = '';
                for (let i = 0; i < data.length; i++) {
                    result += `<div onclick="clickMe('${data[i]}','${productId}');" data-id="${data[i]}" class="size-box">
                         <span style="margin: auto;font-weight:lighter;font-size: 14px;">${data[i]}</span>
                         </div>`;
                }

                $('.product-size__box').html(result);

            }
        });
    });
});

function clickMe(size,id) {
    console.log(size + id);
    $('.product-size__box .size-box').each(function () {
        $(this).css("background-color", "transparent");
        $(this).css("color", "#333");
        if ($(this).attr('data-id') == size) {
            $(this).css("background-color", "#8f7d59");
            $(this).css("color", "#FFFFFF");
        }
    });


    $.ajax({
        type: 'POST',
        data: {
            size: size,
            product_id: id
        },
        url: '/customer/product/findPriceBySizeAndId',
        success: function (result) {
            console.log(result);
            var priceFormatted = result.toLocaleString('it-IT', { style: 'currency', currency: 'VND' });
            $('.product-price').text(priceFormatted.replace('.', ','));
        }
    });
}

// Close modal add to cart

function closeModalAddToCart() {
    var element = document.getElementById("addToCart");
    element.classList.remove('show-modal');
    element.classList.add('hide');
}

// Show size guide

function showSizeGuide() {
    var element = document.getElementById("size-guide__table");
    element.classList.remove('hide');
    element.classList.add('show');
}

// Hide size guide

function hideSizeGuide(event) {
    event.stopPropagation();
    const element = document.querySelector(".size-guide__table");
    element.classList.remove("show");
    element.classList.add('hide');
}

// Open cart for 3s after add product to cart

function timeOutCartDisplay() {
    var cart = document.getElementById("header__cart-list");
    cart.style.display = "none";
}

function showCartDisplay() {
    var cart = document.getElementById("header__cart-list");
    cart.style.display = "block";
    closeModalAddToCart();
}