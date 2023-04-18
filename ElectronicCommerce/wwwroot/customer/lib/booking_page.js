function loadCartDoubleCheck() {
    console.log('star loadCartDoubleCheck');
    $.ajax({
        type: 'GET',
        url: '/customer/product/findAllCart',
        success: function (cart) {
            console.log(cart);
            if (cart != null && cart.length > 0) {
                var result = ``;
                var total = 0;
                for (let i = 0; i < cart.length; i++) {
                    if (cart[i].isCheck) {
                        var price = 0;
                        if (cart[i].savePrice > 0) {
                            price = cart[i].price - cart[i].savePrice;
                        } else {
                            price = cart[i].price;
                        }

                        var formatPrice = price.toLocaleString('it-IT', { style: 'currency', currency: 'VND' });

                        result += `<div class="cart-item-box">
                                <img src="/admin/images/products/${cart[i].image}" alt="" style="width:135px;height:180px;" />
                                <div class="cart-quantity">
                                    <span style="font-size:12px;font-weight:bolder;">x ${cart[i].quantity}</span>
                                </div>
                                <div class="product-detail-wrapper">
                                    <span style="text-overflow:ellipsis;color:#999;margin-top:6px;">${cart[i].name}</span>
                                    <span style="margin-top:6px;">${formatPrice}</span>
                                </div>
                            </div>`;
                        total += (price * cart[i].quantity);
                    }
                }

                $('.cart-detail-wrapper').html(result);

                // Tong tien gio hang

                var totalFormatted = total.toLocaleString('it-IT', { style: 'currency', currency: 'VND' })
                $('#total-booking-price').text(totalFormatted);
            } 
        }
    });

    console.log('End loadCartDoubleCheck');
}