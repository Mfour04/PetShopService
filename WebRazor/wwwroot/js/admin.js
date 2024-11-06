document.addEventListener("DOMContentLoaded", function () {
    loadTable('users');
});

async function loadTable(page) {
    // Clear any existing table content
    document.getElementById("table-container").innerHTML = "";

    // Define table data based on the selected page
    let tableContent = "";
    switch (page) {
        case "users":
            const response = await fetch('/Shared/Admin/AdminHome?handler=Users');
            const users = await response.json();
            tableContent = `
                    <h3>User Data</h3>
                    <table class="table table-bordered">
                        <thead>
                                <tr class="bg-primary2 border n20-1-border radius-8">
                                    <th class="p-xxl-6 p-lg-4 p-2">ID</th>
                                    <th class="p-xxl-6 p-lg-4 p-2 text-center">Name</th>
                                    <th class="p-xxl-6 p-lg-4 p-2 text-center">Phone</th>
                                    <th class="p-xxl-6 p-lg-4 p-2 text-center">Email</th>
                                    <th class="p-xxl-6 p-lg-4 p-2 text-center">Address</th>
                                </tr>
                        </thead>
                        <tbody>
                `;
            users.forEach(user => {
                tableContent +=
                `
                    <tr>
                        <td>${user.userId}</td>
                        <td>Tai</td>
                        <td>${user.phone}</td>
                        <td>${user.email}</td>
                        <td>${user.address}</td>
                    </tr>
                `
            })
            tableContent += `</tbody></table>`
            break;
        case "products":
            const productsResponse = await fetch('/Shared/Admin/AdminHome?handler=Products');
            const products = await productsResponse.json();
            tableContent = `
                    <h3>User Data</h3>
                    <table class="table table-bordered">
                        <thead>
                                <tr class="bg-primary2 border n20-1-border radius-8">
                                    <th class="p-xxl-6 p-lg-4 p-2">ID</th>
                                    <th class="p-xxl-6 p-lg-4 p-2 text-center">Name</th>
                                    <th class="p-xxl-6 p-lg-4 p-2 text-center">Price</th>
                                </tr>
                        </thead>
                        <tbody>
                `;
            products.forEach(product => {
                tableContent +=
                    `
                    <tr>
                        <td>${product.productId}</td>
                        <td>${product.productName}</td>
                        <td>${product.price}</td>
                    </tr>
                `
            })
            tableContent += `</tbody></table>`
            break;
        default:
            tableContent = "<p>No data available.</p>";
    }

    // Insert the generated table into the table container
    document.getElementById("table-container").innerHTML = tableContent;
}