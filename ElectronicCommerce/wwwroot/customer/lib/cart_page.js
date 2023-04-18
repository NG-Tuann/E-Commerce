function loadCartSummary() {
    console.log('star loadCartSummary');
    $.ajax({
        type: 'GET',
        url: '/customer/product/findAllCart',
        success: function (cart) {
            console.log(cart);
            if (cart != null && cart.length >0) {
                $('.cart__summary-title').text(`Tóm Tắt Mặt Hàng(${cart.length})`);
                console.log("Cart length: " + cart.length);
                // Hien thi cart detail

                var emptyCart = document.getElementById("empty-cart");
                emptyCart.style.display = "none";

                var cartDetail = document.getElementById("cart-detail");
                cartDetail.style.display = "block";
                var totalSave = 0;
                var result = ``;
                var total = 0;

                for (let i = 0; i < cart.length; i++) {
                    var priceFormatted = cart[i].price.toLocaleString('it-IT', { style: 'currency', currency: 'VND' })
                    if (cart[i].isCheck) {
                        total += cart[i].price * cart[i].quantity;
                        totalSave += cart[i].savePrice * cart[i].quantity;
                    }

                    else {
                        $('.paid-check-all').prop("checked", false);
                    }
                    var subTotal = (cart[i].price * cart[i].quantity).toLocaleString('it-IT', { style: 'currency', currency: 'VND' });

                    result += `<div class="cart__summary-item">
                            <div class="cart__over-wrapper" style="display: flex;">
                                <span style="position: relative;">
                                    <label class="checkbox-container">
                                        <input onclick="checkPaidProduct('${cart[i].product.productDetailId}');" class="paid-product" data-id="${cart[i].product.productDetailId}" id="check-${cart[i].product.productDetailId}" type="checkbox" ${cart[i].isCheck ? `checked="checked"` : ``}>
                                        <span class="checkmark" style="position: absolute;top:40px;"></span>
                                    </label>
                                </span>
                            </div>
                            <div class="item-image">
                                <img src="/admin/images/products/${cart[i].image}" width="120" height="100">
                            </div>
                            <div class="cart__summary-info">
                                <div>
                                    <span style="font-size: 13px;">${cart[i].name}</span>
                                </div>
                                <div class="cart__summary-info-value">
                                    <span style="font-size: 13px;">Size sản phẩm: ${cart[i].size}</span>
                                    <span>${priceFormatted}</span>
                                    <span>
                                        <div class="input__quantity-wrapper">
                                            ${cart[i].quantity == 1 ? `<div class="minus_item" id="minus-item-disable">-</div>` : `<div class="minus_item" onclick="updateQuantity('${cart[i].product.productDetailId}','minus');">-</div>`}
                                            <div class="quantity_result">
                                                ${cart[i].quantity}
                                            </div>
                                            <div class="add_item" onclick="updateQuantity('${cart[i].product.productDetailId}','plus');">+</div>
                                        </div>
                                    </span>
                                    <span>${subTotal}</span>
                                </div>
                                <div>
                                    <span style="font-size: 13px;">Xoá</span>
                                </div>
                            </div>
                        </div>`
                }

                $('.cart__summary-item-wrapper').html(result);

                // Tong tien gio hang

                var totalFormatted = total.toLocaleString('it-IT', { style: 'currency', currency: 'VND' })
                $('.cart-total').text(totalFormatted);
                $('#totalSave').text('Tiết kiệm: '+totalSave.toLocaleString('it-IT', { style: 'currency', currency: 'VND' }));

            } else {
                var emptyCart = document.getElementById("empty-cart");
                emptyCart.style.display = "block";

                var cartDetail = document.getElementById("cart-detail");
                cartDetail.display = "none";
            }
        }
    });

    console.log('End loadCart');
}

function updateQuantity(id, action) {
    // Update quantity va load lai cart
    $('#pre-loader').show();
    $.ajax({
        type: 'POST',
        async: false,
        data: {
            product_detail_id: id,
            action: action
        },
        url: '/customer/cart/updateQuantity',
        success: function (result) {
            setTimeout(function () {
                $('#pre-loader').hide();
                if (result.message == 'Outstock') {
                    alert("Out of stock !");
                    console.log("out of stock");
                }
                else {
                    console.log(result);
                    loadCartSummary();
                }
            }, 800);
        }
    });
}

// CHECK COMMIT PAID ON CART

function checkAllPaidProduct() {
    console.log('check all');
    // Kiem tra trang thai truoc khi nhan

    $('#pre-loader').show();

    setTimeout(function () {
        if ($('.paid-check-all').is(':checked')) {
            // Lap qua cac checkbox va set trang thai la checked cho moi checkbox

            loopCheckBox(true);

            // Tinh lai tong tien gio hang

            loadCartSummary();
        }
        else {
            console.log("uncheck");
            // Lap qua cac checkbox va set trang thai la checked cho moi checkbox

            loopCheckBox(false);

            // Tinh lai tong tien gio hang

            loadCartSummary();
        }
        $('#pre-loader').hide();
    },800);
}

// Lap qua va check cac checkbox

function loopCheckBox(isCheck) {

    $('.paid-product').each(function () {
        var productDetailId = $(this).attr('data-id');

        // Set state  checked or not for checkbox

        $(this).prop("checked", isCheck);

        $.ajax({
            type: 'POST',
            async: false,
            data: {
                product_detail_id: productDetailId,
                isCheck: isCheck
            },
            url: '/customer/cart/updatePaidProduct',
            success: function (result) {
                console.log(result);
            }
        });
    });
}

// Kiem tra nguoi dung chon san pham nao de thanh toan

function checkPaidProduct(id) {

    $('#pre-loader').show();

    var isAllCheck = true;

    // Kiem tra set tick cho nut check all

    $('.paid-product').each(function () {
        if (!($(this).is(':checked'))) {
            console.log('1 un check');
            isAllCheck = false;
        }
    });

    console.log(isAllCheck);

    tickTheCheckAll(isAllCheck);

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
    setTimeout(function () {
        loadCartSummary();
        $('#pre-loader').hide();
    },800);
}

// Chon vao check all

function tickTheCheckAll(isCheck) {
    $('.paid-check-all').prop("checked", isCheck);
}

