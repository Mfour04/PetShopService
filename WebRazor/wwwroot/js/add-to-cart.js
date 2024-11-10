function handleClickAddToCart(product) {
    let cart = [];

    //cart.push(productId);
    if (sessionStorage.getItem('cart')) {
        // Nếu có, lấy giá trị hiện tại từ sessionStorage
        cart = JSON.parse(sessionStorage.getItem('cart'));
    }

    cart.push({
        ProductId: product.ProductId, 
        ProductName: product.ProductName,
        Price: product.Price,
        quantity: 1 
    });
    sessionStorage.setItem('cart', JSON.stringify(cart));
    updateCartCount();
    console.log(cart);
}

function updateCartCount() {
    const cartCountElement = document.getElementById('cart-count');
    let cart = JSON.parse(sessionStorage.getItem('cart')) || [];
    const totalItems = cart.reduce((acc, item) => acc + item.quantity, 0);

    if (cartCountElement) {
        cartCountElement.textContent = totalItems;
    }
}

window.addEventListener('load', () => {
    const cartCountElement = document.getElementById('cart-count');
    const savedTotalItems = localStorage.getItem('totalItems');

    if (savedTotalItems && cartCountElement) {
        cartCountElement.textContent = savedTotalItems;
    }

    updateCartCount();  
});

function updatePriceLabels() {
    document.querySelector('.min-label').textContent = document.querySelector('input[type="range"].min').value;
    document.querySelector('.max-label').textContent = document.querySelector('input[type="range"].max').value;
}

