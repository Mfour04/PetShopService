document.addEventListener('DOMContentLoaded', function () {
    const cart = sessionStorage.getItem('cart');
    if (cart) {

        const cartList = JSON.parse(cart);
        // find element <ul> in HTML to assign <li>
        const cartListElement = document.getElementById('cartList');
        const totalSubtotalInput = document.getElementById('totalSubtotalInput'); 
        let totalSubtotal = 0;
        console.log(cartListElement);
        // Loop through products and create <li> for each products
        cartList.forEach(cart => {
            //Start render 
            const newRow = document.createElement('tr');
            newRow.setAttribute('key', cart.ProductId)
            newRow.className = "align-items-center border-bottom n20-1-border";
            const subtotal = cart.Price * cart.quantity;
            totalSubtotal += subtotal;
            console.log(totalSubtotal);
            newRow.innerHTML =
                `
        <td class="p-xxl-6 p-lg-4 p-2">
            <a href="shop-details.html" class="d-flex align-items-center gap-3">
                <img class="w-100 icon-80px radius-unset" src="/images/product-1.png" alt="product image">
                <span class="text-n20 fw-medium font-instrument">${cart.ProductName}</span>
            </a>
        </td>
        <td class="p-xxl-6 p-lg-4 p-2 fw-medium font-instrument text-center">${cart.Price}</td>
        <td class="p-xxl-6 p-lg-4 p-2 text-center">
            <div class="quantity d-inline-flex align-items-center justify-content-center gap-2 py-lg-3 py-2 px-4 border n20-1-border radius-pill">
                <button class="quantityDecrement text-primary">
                    <i class="ph-fill ph-minus"></i>
                </button>
                <input type="text" value="${cart.quantity}" value="1" class="quantityValue border-0 p-0 outline-0">
                <button class="quantityIncrement text-primary">
                    <i class="ph-fill ph-plus"></i>
                </button>
            </div>
        </td>
        <td class="p-xxl-6 p-lg-4 p-2 fw-medium font-instrument text-center subtotal">${subtotal}</td>
        <td class="p-xxl-6 p-lg-4 p-2 text-center w-100px">
            <button class="cart-prod-remove-btn fw-medium font-instrument"
                    onclick="removeItemFromCart('${cart.ProductId}')">
                <i class="ph ph-x"></i>
            </button>
        </td>
    `;
            cartListElement.appendChild(newRow);
            const quantityInput = newRow.querySelector('.quantityValue');
            const subtotalElement = newRow.querySelector('.subtotal');
            attachQuantityHandlers(cart, cartList, newRow, quantityInput, subtotalElement);
        });
        totalSubtotalInput.value = totalSubtotal;  
    } else {
        console.log("No products found in sessionStorage.");
    }
});
function removeItemFromCart(ProductId) {
    let cart = sessionStorage.getItem('cart');

    if (cart) {
        const cartList = JSON.parse(cart);
        const updatedCart = cartList.filter(item => item.ProductId !== ProductId);
        console.log(updatedCart)
        sessionStorage.setItem('cart', JSON.stringify(updatedCart));
        console.log(`Product "${ProductId}" is removed.`);
    } else {
        console.log("Empty Cart");
    }
}

function attachQuantityHandlers(cart, cartList, newRow, quantityInput, subtotalElement) {
    const decrementButton = newRow.querySelector('.quantityDecrement');
    const incrementButton = newRow.querySelector('.quantityIncrement');

    // update subtotal function
    const updateSubtotal = () => {
        const newQuantity = parseInt(quantityInput.value);
        if (isNaN(newQuantity) || newQuantity <= 0) {
            quantityInput.value = 1; 
        }
        const updatedSubtotal = (cart.Price * quantityInput.value);
        subtotalElement.textContent = updatedSubtotal;

        // update quantity in sessionStorage
        cart.quantity = parseInt(quantityInput.value);
        sessionStorage.setItem('cart', JSON.stringify(cartList));
        updateTotalSubtotal(cartList);
    };

    let isClickEvent = false;

    // decrease quantity event
    decrementButton.addEventListener('click', () => {
        isClickEvent = true;
        if (quantityInput.value > 1) {
            quantityInput.value = parseInt(quantityInput.value) - 1;
            updateSubtotal();
        }
        isClickEvent = false;
    });

    // Increase quantity event
    incrementButton.addEventListener('click', () => {
        isClickEvent = true;
        quantityInput.value = parseInt(quantityInput.value) + 1;
        updateSubtotal();
        isClickEvent = false;
    });

}

function updateTotalSubtotal(cartList) {
    let totalSubtotal = 0;

    cartList.forEach(cart => {
        totalSubtotal += cart.Price * cart.quantity;
    });

    const totalSubtotalInput = document.getElementById('totalSubtotalInput');
    totalSubtotalInput.value = totalSubtotal;

    console.log("Total subtotal updated:", totalSubtotal);
}