// Show modal choose finger size
function loadCart() {
    console.log('star loadCart');
    $.ajax({
        type: 'GET',
        url: '/customer/product/findAllCart',
        success: function (cart) {
            console.log(cart);
            if (cart != null) {
                console.log("Cart length: "+cart.length);
                var result = ``;
                var total = 0;

                for (let i = 0; i < cart.length; i++) {
                    var priceFormatted = cart[i].price.toLocaleString('it-IT', { style: 'currency', currency: 'VND' })
                    if (cart[i].isCheck) {
                        total += cart[i].price * cart[i].quantity;
                    }
                    result += `<li class="header__cart-item">
                            <div class="cart__over-wrapper" style="display:flex;margin-left:18px;margin-right:20px;">
                                <span style="position: relative;">
                                    <label class="checkbox-container">
                                        <input onclick="checkPaidProduct('${cart[i].product.productDetailId}');" class="paid-product" data-id="${cart[i].product.productDetailId}" id="check-${cart[i].product.productDetailId}" type="checkbox" ${cart[i].isCheck ? `checked="checked"` : ``}>
                                        <span class="checkmark" style="position: absolute;top:-9px;"></span>
                                    </label>
                                </span>
                            </div>
                   <img src="/admin/images/products/${cart[i].image}" alt="Watch" class="header__cart-img" />
                             <div class="header__cart-item-info">
                             <div class="header__cart-item-head">
                             <h5 class="header__cart-item-name">${cart[i].name}</h5>
                                                                            <div class="header__cart-item-price-wrap">
                                                                                <span class="header__cart-item-price">${priceFormatted}đ</span>
                                                                                <span class="header__cart-item-multiply">x</span>
                                                                                <span class="header__cart-item-quantity">${cart[i].quantity}</span>
                                                                            </div>
                                                                        </div>

                                                                        <div class="header__cart-item-body">
                                                                            <span class="header__cart-item-description">
                                                                                Kích cỡ sản phẩm đã chọn: ${cart[i].size}
                                                                            </span>
                                                                            <span class="header__cart-item-remove" onclick="removeItemFromCart(event,'${cart[i].product.productDetailId}');">Xoá</span>
                                                                        </div>
                                                                    </div>
                                                                </li>`
                }

                $('.header__cart-list-item').html(result);

                // Tong tien gio hang

                var totalFormatted = total.toLocaleString('it-IT', { style: 'currency', currency: 'VND' })
                $('.checkout-total-money').text(totalFormatted);

                // So luong san pham trong gio hang
                $('#cartQuantity').text(cart.length);

                var noCart = document.getElementById("header__cart-no-cart");
                noCart.style.display = "none";

                var checkOut = document.getElementById("header__checkout");
                checkOut.style.display = "block";

                var notEmptyCart = document.getElementById("not-empty-cart");
                notEmptyCart.style.display = "block";

            } else {
                var noCart = document.getElementById("header__cart-no-cart");
                noCart.style.display = "block";

                var notEmptyCart = document.getElementById("not-empty-cart");
                notEmptyCart.style.display = "none";

                var checkOut = document.getElementById("header__checkout");
                checkOut.style.display = "none";

                // So luong san pham trong gio hang bang 0
                $('#cartQuantity').text(0);
            }
        }
    });

    console.log('End loadCart');
}

function removeItemFromCart(e, id) {
    e.preventDefault();
    console.log("product detail id: " + id);
    removeItem(id);
}

function removeItem(id) {
    $.ajax({
        type: 'POST',
        data: {
            product_detail_id: id
        },
        url: '/customer/product/removeFromCart',
        success: function (result) {
            console.log(result);
            loadCart();
        }
    });
}

function showModalFingerSize() {
    var element = document.getElementById("fingerSizeModal");
    element.classList.remove('hide');
    element.classList.add('show-modal');
}

// Show modal choose wrist size
function showModalWristSize() {
    var element = document.getElementById("wristSizeModal");
    element.classList.remove('hide');
    element.classList.add('show-modal');
}

// Confirm add to cart after choose size for product or not !IMPORTANT

function confirmAddToCart(id, size) {
    // Goi ajax nhan ve cart session
    $.ajax({
        type: 'POST',
        async: false,
        data: {
            size: size,
            product_id: id
        },
        url: '/customer/product/AddToCart',
        success: function (cart) {
            if (cart.message == 'Outstock') {
                alert("Out of stock !");
                console.log("out of stock");
            }
            else {
                console.log(cart);
                loadCart();
            }
        }
    });

    // Hien thi cart trong vong 3s

    showCartDisplay();
    setTimeout(timeOutCartDisplay, 3000);
    console.log(id);
    console.log(size);
}


$(document).ready(function () {
    // Click on button add to cart

    $('.btn-add-to-cart').click(function () {
        // Lay ve product id

        var productId = $(this).attr('data-id');
        console.log(productId);

        // Kiem tra day co phai la san pham can chon size

        $.ajax({
            type: 'POST',
            async: false,
            data: {
                product_id: productId
            },
            url: '/customer/product/findProductById',
            success: function (product) {
                if (product.name.startsWith("Nhẫn") || product.name.startsWith("Vòng")) {

                    if (product.name.startsWith("Nhẫn")) {
                        console.log("Show nhan size guide");
                        showModalFingerSize();
                    }
                    else if (product.name.startsWith("Vòng")) {
                        console.log("Show vong size guide");
                        showModalWristSize();
                    }
                    // Ajax hien thi san pham tuong ung vao modal

                    console.log(product);
                    $('.product-name').text(product.name);
                    $('.product-id').text('Mã sản phẩm: ' + product.producT_ID);
                    $('.product-thumbnail').attr('src', '/admin/images/products/' + product.image);
                    console.log(product.image);
                    var priceFormatted = product.price.toLocaleString('it-IT', { style: 'currency', currency: 'VND' });
                    $('.product-price').text(priceFormatted.replace('.', ','));

                    // Tim ve tat ca size cua san pham tuong ung
                    findAllSizeOfProducts(productId);

                }
                else {
                    console.log("Day khong can chon size");
                    confirmAddToCart(productId, 0);
                }

            }
        });
    });

    // Xac nhan them vao gio hang sau khi chon size

    $('.btn-add-to-cart__confirm').click(function () {
    console.log('click');
    var size = $('#size').val();
    var id = $('#product-id').val();

    if (size == 0) {
        alert('Vui lòng chọn size của sản phẩm theo bảng chỉ dẫn chọn size !');
    }
    else {
        $('#size').val(0);
        $('#product-id').val(0);
        confirmAddToCart(id, size);
    }
});
});


function findAllSizeOfProducts(productId) {
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
                result += `<div onclick="clickChooseSize('${data[i]}','${productId}');" data-id="${data[i]}" class="size-box">
                         <span style="margin: auto;font-weight:lighter;font-size: 14px;">${data[i]}</span>
                         </div>`;
            }

            $('.product-size__box').html(result);

        }
    });
}

function clickChooseSize(size,id) {
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

    $('#size').val(size);
    $('#product-id').val(id);
}

// Close modal finger size

function closeModalFingerSize() {
    var element = document.getElementById("fingerSizeModal");
    element.classList.remove('show-modal');
    element.classList.add('hide');
}

// Close modal wrist size

function closeModalWristSize() {
    var element = document.getElementById("wristSizeModal");
    element.classList.remove('show-modal');
    element.classList.add('hide');
}

// Show finger size guide

function showFingerSizeGuide() {
    var element = document.getElementById("size-guide__table");
    element.classList.remove('hide');
    element.classList.add('show');
}

// Hide finger size guide

function hideFingerSizeGuide(event) {
    event.stopPropagation();
    const element = document.querySelector(".size-guide__table");
    element.classList.remove("show");
    element.classList.add('hide');
}

// Show wrist size guide

function showWristSizeGuide() {
    var element = document.getElementById("size-wrist-guide__table");
    element.classList.remove('hide');
    element.classList.add('show');
}

// Hide wrist size guide

function hideWristSizeGuide(event) {
    event.stopPropagation();
    var element = document.getElementById("size-wrist-guide__table");
    element.classList.remove('show');
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
    closeModalFingerSize();
    closeModalWristSize();
}

// Xu ly nguoi dung chon san pham nao de thanh toan
function checkPaidProduct(id) {
    // Kiem tra nguoi dung chon san pham nao de thanh toan

    $('.paid-product').each(function () {
        if ($(this).attr('data-id') == id) {
            // Cap nhat trang thai isCheck trong cart

            if ($(this).is(':checked')) {
                $.ajax({
                    type: 'POST',
                    async: false,
                    data: {
                        product_detail_id: id,
                        isCheck: true
                    },
                    url: '/customer/cart/updatePaidProduct',
                    success: function (result) {
                        console.log(result);
                    }
                });
            }
            else {
                $.ajax({
                    type: 'POST',
                    async: false,
                    data: {
                        product_detail_id: id,
                        isCheck: false
                    },
                    url: '/customer/cart/updatePaidProduct',
                    success: function (result) {
                        console.log(result);
                    }
                });
            }
        }
    });

    // Load lai cart

    loadCart();
}
