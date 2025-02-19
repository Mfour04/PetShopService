// Hàm thêm sản phẩm vào giỏ hàng
function handleClickAddToCart(product) {
   let cart = [];

   // Kiểm tra nếu đã có giỏ hàng trong sessionStorage
   if (sessionStorage.getItem('cart')) {
      cart = JSON.parse(sessionStorage.getItem('cart'));
   }

   // Thêm sản phẩm vào giỏ hàng
   cart.push({
      ProductId: product.ProductId,
      ProductName: product.ProductName,
      Price: product.Price,
      quantity: 1
   });

   // Lưu giỏ hàng vào sessionStorage
   sessionStorage.setItem('cart', JSON.stringify(cart));

   // Cập nhật số lượng sản phẩm hiển thị trên icon giỏ hàng
   updateCartCount();

   // Hiển thị thông báo Toastr
   showToast(product.ProductName);

   console.log(cart);
}

// Hàm cập nhật số lượng hiển thị trên giỏ hàng
function updateCartCount() {
   const cartCountElement = document.getElementById('cart-count');
   let cart = JSON.parse(sessionStorage.getItem('cart')) || [];
   const totalItems = cart.reduce((acc, item) => acc + item.quantity, 0);

   if (cartCountElement) {
      cartCountElement.textContent = totalItems;
   }
}

// Hàm hiển thị thông báo Toastr
function showToast(productName) {
   toastr.success(`${productName} đã được thêm vào giỏ hàng!`, 'Thành công!', {
      timeOut: 3000, // Hiển thị trong 3 giây
      progressBar: true,
      positionClass: "toast-bottom-right",
      showMethod: "fadeIn",
      hideMethod: "fadeOut"
   });
}

// Khi load lại trang, cập nhật số lượng giỏ hàng
window.addEventListener('load', () => {
   updateCartCount();
});

// Hàm cập nhật nhãn giá min/max khi kéo thanh range
function updatePriceLabels() {
   document.querySelector('.min-label').textContent = document.querySelector('input[type="range"].min').value;
   document.querySelector('.max-label').textContent = document.querySelector('input[type="range"].max').value;
}

// Khi người dùng thay đổi thanh trượt, cập nhật giá trị hiển thị
document.querySelector('input[type="range"].min').addEventListener('input', updatePriceLabels);
document.querySelector('input[type="range"].max').addEventListener('input', updatePriceLabels);
