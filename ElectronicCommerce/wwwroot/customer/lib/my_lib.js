// Show modal add to cart 

function addToCart() {
    var element = document.getElementById("addToCart");
    element.classList.remove('hide');
    element.classList.add('show-modal');
    console.log("click count")
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