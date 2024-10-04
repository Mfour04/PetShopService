let cart = [];

function addToCart(button) {
    const product = {
        id: button.getAttribute('data-id'),
        name: button.getAttribute('data-name'),
        price: parseFloat(button.getAttribute('data-price')),
        quantity: parseInt(button.getAttribute('data-quantity'))
    };

    $.ajax({
        url: '/Cart/AddToCart',  // URL của phương thức sẽ xử lý việc thêm vào giỏ hàng
        type: 'POST',
        data: JSON.stringify(product),  // Chuyển product thành chuỗi JSON
        contentType: 'application/json',
        success: function (response) {
            console.log(`${product.name} đã được thêm vào giỏ hàng.`);
            updateCartCount(); // Cập nhật số lượng item trong giỏ
            displayCart();     // Hiển thị giỏ hàng
        },
        error: function (xhr, status, error) {
            console.error("Có lỗi xảy ra: " + error);
        }
    });
}


function updateCartCount() {
    const cartCountElement = document.getElementById('cart-count');
    const totalItems = cart.reduce((acc, item) => acc + item.quantity, 0);
    if (cartCountElement) {
        cartCountElement.textContent = totalItems;
    }
}

function displayCart() {
    const cartTableBody = document.querySelector('#cart-items tbody');
    const cartTotalElement = document.getElementById('cart-total');

    // Clear the table first
    cartTableBody.innerHTML = '';

    let totalPrice = 0;

    // Check if the cart is empty
    if (cart.length === 0) {
        cartTableBody.innerHTML = '<tr><td colspan="4" class="text-center">Your cart is empty</td></tr>';
    } else {
        // Loop through the cart items and create table rows
        cart.forEach(item => {
            const itemTotal = (item.price * item.quantity).toFixed(2);
            totalPrice += parseFloat(itemTotal);

            // Create a row for each cart item
            const row = `
                <tr>
                    <td>${item.name}</td>
                    <td>${item.quantity}</td>
                    <td>$${item.price.toFixed(2)}</td>
                    <td>$${itemTotal}</td>
                </tr>
            `;

            // Append the row to the table body
            cartTableBody.innerHTML += row;
        });
    }

    // Display the total price
    cartTotalElement.textContent = `Total: $${totalPrice.toFixed(2)}`;
}

