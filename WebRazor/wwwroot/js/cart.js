document.addEventListener('DOMContentLoaded', function () {
    const cart = sessionStorage.getItem('cart');
    if (cart) {
        const cartList = JSON.parse(cart);
        updateTotalSubtotal(cartList);
        const cartListElement = document.getElementById('cartList');
        cartListElement.innerHTML = ''; 

        cartList.forEach(cart => {
            const newRow = createCartRow(cart);
            cartListElement.appendChild(newRow);
        });

        sessionStorage.setItem('cart', JSON.stringify(cartList));
    }
});
function createCartRow(cart) {
    const newRow = document.createElement('tr');
    newRow.setAttribute('key', cart.ProductId);
    newRow.className = "align-items-center border-bottom n20-1-border";

    const subtotal = cart.Price * cart.quantity;

    const formatCurrency = (value) => {
        return value.toLocaleString('vi-VN').replaceAll(',', '.') + " VND";
    };

    newRow.innerHTML = `
        <td class="p-xxl-6 p-lg-4 p-2">
            <a href="shop-details.html" class="d-flex align-items-center gap-3">
                <img class="w-100 icon-80px radius-unset" src="/images/product-1.png" alt="product image">
                <span class="text-n20 fw-medium font-instrument">${cart.ProductName}</span>
            </a>
        </td>
        <td class="p-xxl-6 p-lg-4 p-2 fw-medium font-instrument text-center">${formatCurrency(cart.Price)}</td>
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
        <td class="p-xxl-6 p-lg-4 p-2 fw-medium font-instrument text-center subtotal">${formatCurrency(subtotal)}</td>
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

        cart.quantity = newQuantity;
        sessionStorage.setItem('cart', JSON.stringify(cartList));

        subtotalElement.textContent = formatCurrency(cart.Price * newQuantity);

        updateTotalSubtotal(cartList);
    };

    decrementButton.addEventListener('click', () => {
        let currentQuantity = parseInt(quantityInput.value);
        if (currentQuantity > 1) {
            quantityInput.value = currentQuantity - 1;
            updateSubtotal();
        }
    });

    incrementButton.addEventListener('click', () => {
        let currentQuantity = parseInt(quantityInput.value);
        quantityInput.value = currentQuantity + 1;
        updateSubtotal();
    });
}

document.addEventListener('click', function (event) {
    if (event.target.closest('.quantityIncrement')) {
        changeQuantity(event.target.closest('.quantityIncrement'), 1);
    } else if (event.target.closest('.quantityDecrement')) {
        changeQuantity(event.target.closest('.quantityDecrement'), -1);
    }
});
function changeQuantity(button, change) {
    const row = button.closest('tr');
    const quantityInput = row.querySelector('.quantityValue');
    const subtotalElement = row.querySelector('.subtotal');
    const productId = row.getAttribute('key');
    const formatCurrency = (value) => {
        return value.toLocaleString('vi-VN').replaceAll(',', '.') + " VND";
    };
    let cart = JSON.parse(sessionStorage.getItem('cart')) || [];
    let cartItem = cart.find(item => item.ProductId == productId);

    if (cartItem) {
        
        let newQuantity = cartItem.quantity + change;
        if (newQuantity < 1) newQuantity = 1;

        cartItem.quantity = newQuantity;
        quantityInput.value = newQuantity;
        subtotalElement.textContent = formatCurrency(cartItem.Price * newQuantity);

        sessionStorage.setItem('cart', JSON.stringify(cart));
        updateTotalSubtotal(cart);

    }
}
function updateTotalSubtotal(cartList) {
    let totalSubtotal = 0;

    cartList.forEach(cart => {
        totalSubtotal += cart.Price * cart.quantity;
    });

    const formatCurrency = (value) => {
        return value.toLocaleString('vi-VN').replaceAll(',', '.') + " VND";
    };

    const totalSubtotalDisplay = document.getElementById('totalSubtotalDisplay');
    const totalSubtotalInput = document.getElementById('totalSubtotalInput');

    totalSubtotalDisplay.textContent = formatCurrency(totalSubtotal);
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
