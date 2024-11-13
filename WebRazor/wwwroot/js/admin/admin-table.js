

function openEditModal(itemId, Name) {


    const row = document.querySelector(`tr[accesskey='${Name}-${itemId}']`)

    const cells = Array.from(row.getElementsByTagName('td'))
    const values = Array.from(row.getElementsByTagName('input'))




    const items = cells.reduce((acc, cell) => {
        const name = cell.getAttribute('data-name')
        const value = cell.textContent.trim()
        if (name !== 'none') {
            acc[name] = value
        }
        return acc
    }, {})


    const modal = document.getElementById('editModal')
    modal.dataset.accessKey = itemId
    modal.dataset.name = Name
    const modalContent = document.getElementById('editForm');

    Object.entries(items).map(([key, value]) => {
        const keys = key.split('-')
        const type = keys[0]
        const container = document.createElement('div');
        container.classList.add('form-container')

        const label = document.createElement('label');
        label.textContent = keys[1].charAt(0).toUpperCase() + keys[1].slice(1); 
        label.setAttribute('for', keys[1]);
        label.classList.add('edit-label')
        container.appendChild(label);

        switch (type) {
            case 'Input':
                const input = createInput(keys[1], value)
                container.appendChild(input)
                break;
            case 'Radio':
                const radio = createRadio(keys[1], value)
                container.appendChild(radio)
                break;
        }

        //const input = document.createElement('input');
        //input.type = 'text'; 
        //input.id = key; 
        //input.name = key; 
        //input.value = value;
        //input.classList.add('edit-input')

        modalContent.appendChild(container);
        modalContent.appendChild(document.createElement('br'));
    })

    modal.style.display = 'block'

}

function closeEditModal() {
    const modal = document.getElementById("editModal")

    modal.style.display = 'none'
    modal.removeAttribute('accesskey')

    const form = document.getElementById("editForm")
    while (form.firstChild) {
        form.removeChild(form.firstChild)
    }
}

function submitEditForm() {
    const modal = document.getElementById("editModal")


    const modalContent = document.getElementById('editForm');
    const inputs = modalContent.querySelectorAll('div > label + input');

    let formData = Array.from(inputs).reduce((obj, input) => {
        const name = input.getAttribute('name'); 
        if (input.type === 'radio') {
            if (input.value === 'true') {
                const value = true;
                obj[name] = value;
                return obj;
            } else {
                const value = false;
                obj[name] = value;
                return obj;
            }
        } else {
            const value = input.value;
            obj[name] = value;
            return obj;
        }
    }, {});

    formData.userId = modal.dataset.accessKey


    const formTemp = {
        UserId: 1,          // or 0 for new user
        RoleId: "admin",    // optional, if needed
        Email: "admin@petshop.com",
        Password: "123456",
        Phone: "123456780",
        Address: "123 Main St",
        Gender: true         // true or false (since it's a bool?)
    };

    fetch('/Admin/Table', {
        method: 'POST',
        headers: {
            RequestVerificationToken: document.getElementsByName("__RequestVerificationToken")[0].value,
            'Content-Type': 'application/json',
            Accept: 'application/json',
        },
        body: JSON.stringify(formData)
    })
    //    .then(response => response.json())
    //    .then(data => {
    //        if (data.success) {
    //        } else {
    //        }
    //    })
    //    .catch(error => {
    //        console.error('Error:', error);
    //});

    closeEditModal();
    window.location.reload()
}

const createInput = (key, value) => {
    const input = document.createElement('input');
    input.type = 'text';
    input.id = key;
    input.name = key;
    input.value = value;
    input.classList.add('edit-input')
    return input
}

const createRadio = (key, value) => {

    let checked

    if (value === "Male") checked = true
    else checked = false

    const radio = document.createElement('input');
    radio.type = 'radio';
    radio.id = key;
    radio.name = key;
    radio.value = checked;
    radio.checked = checked;
    radio.classList.add('edit-input');
    radio.addEventListener('click', () => {
        checked = !checked
        radio.checked = checked
        radio.value = checked
    })

    //if (value) {
    //    radio.checked = true; 
    //}
    return radio;
}

function editProductModal(productId) {
    const modal = document.getElementById("editProductModal")
    modal.style.display = 'block'
}