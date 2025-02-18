document.addEventListener('DOMContentLoaded', function () {
    const cart = sessionStorage.getItem('cart');
    if (cart) {
        const cartList = JSON.parse(cart);
        updateTotalSubtotal(cartList); 
        const cartListElement = document.getElementById('cartList');
        const totalSubtotalInput = document.getElementById('totalSubtotalInput');
        let totalSubtotal = 0;

        cartList.forEach(cart => {
            const newRow = createCartRow(cart);
            cartListElement.appendChild(newRow);
            totalSubtotal += cart.Price * cart.quantity;
            attachQuantityHandlers(cart, cartList, newRow);
        });

        totalSubtotalInput.value = totalSubtotal.toFixed(2);
        updateTotalSubtotal(cartList);
    } else {
        console.log("No products found in sessionStorage.");
    }
});

function createCartRow(cart) {
    const newRow = document.createElement('tr');
    newRow.setAttribute('key', cart.ProductId);
    newRow.className = "align-items-center border-bottom n20-1-border";

    const subtotal = cart.Price * cart.quantity;

    newRow.innerHTML = `
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
                <input type="text" value="${cart.quantity}" class="quantityValue border-0 p-0 outline-0">
                <button class="quantityIncrement text-primary">
                    <i class="ph-fill ph-plus"></i>
                </button>
            </div>
        </td>
        <td class="p-xxl-6 p-lg-4 p-2 fw-medium font-instrument text-center subtotal">${subtotal}</td>
        <td class="p-xxl-6 p-lg-4 p-2 text-center w-100px">
            <button class="cart-prod-remove-btn fw-medium font-instrument" onclick="removeItemFromCart('${cart.ProductId}')">
                <i class="ph ph-x"></i>
            </button>
        </td>
    `;

    return newRow;
}

function attachQuantityHandlers(cart, cartList, newRow) {
    const quantityInput = newRow.querySelector('.quantityValue');
    const subtotalElement = newRow.querySelector('.subtotal');
    const decrementButton = newRow.querySelector('.quantityDecrement');
    const incrementButton = newRow.querySelector('.quantityIncrement');
    const updateSubtotal = () => {
        let newQuantity = parseInt(quantityInput.value);
        if (isNaN(newQuantity) || newQuantity <= 0) {
            newQuantity = 1;
            quantityInput.value = 1;
        }

        const updatedSubtotal = cart.Price * newQuantity;
        subtotalElement.textContent = updatedSubtotal.toFixed(2);


        cart.quantity = newQuantity;

        sessionStorage.setItem('cart', JSON.stringify(cartList));

        updateTotalSubtotal(cartList);
    };

    decrementButton.addEventListener('click', () => {
        let currentQuantity = parseInt(quantityInput.value);
        if (currentQuantity > 1) {
            currentQuantity -= 1;
            quantityInput.value = currentQuantity; 
            updateSubtotal();
        }
    });

    incrementButton.addEventListener('click', () => {
        let currentQuantity = parseInt(quantityInput.value);
        currentQuantity += 1;
        quantityInput.value = currentQuantity;
        updateSubtotal();
    });
}
document.addEventListener('DOMContentLoaded', function () {
    const cart = sessionStorage.getItem('cart');
    if (cart) {
        const cartList = JSON.parse(cart);
        const productIdsContainer = document.getElementById('productIdsContainer');

        cartList.forEach(cart => {
            const productIdInput = document.createElement('input');
            productIdInput.type = 'hidden';
            productIdInput.name = 'ProductIds'; 
            productIdInput.value = cart.ProductId; 

            productIdsContainer.appendChild(productIdInput);
        });
       
    }
});


function updateTotalSubtotal(cartList) {
    let totalSubtotal = 0;

    cartList.forEach(cart => {
        totalSubtotal += cart.Price * cart.quantity;
    });

    const totalSubtotalDisplay = document.getElementById('totalSubtotalDisplay');
    const totalSubtotalInput = document.getElementById('totalSubtotalInput');

    totalSubtotalDisplay.textContent = totalSubtotal.toFixed(2);  
    totalSubtotalInput.value = Math.round(totalSubtotal);
}
document.addEventListener('DOMContentLoaded', function () {
    const cart = sessionStorage.getItem('cart');
    if (cart) {
        const cartList = JSON.parse(cart);
        updateTotalSubtotal(cartList);
    }
});

function removeItemFromCart(ProductId) {
    let cart = sessionStorage.getItem('cart');

    if (cart) {
        const cartList = JSON.parse(cart);
        const updatedCart = cartList.filter(item => item.ProductId !== ProductId);
        sessionStorage.setItem('cart', JSON.stringify(updatedCart));
        console.log(`Product "${ProductId}" is removed.`);
        location.reload();
    } else {
        console.log("Empty Cart");
    }
}
