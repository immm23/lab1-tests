// API base URL
const baseUrl = 'https://localhost:44377/api/Customers';

// Function to send GET request to retrieve all customers
function getCustomers() {
    fetch(baseUrl)
        .then(response => response.json())
        .then(data => {
            const customersContainer = document.getElementById('customers');
            customersContainer.innerHTML = '';
            data.forEach(customer => {
                const customerDiv = document.createElement('div');
                customerDiv.textContent = `ID: ${customer.id}, Name: ${customer.name}, Surname: ${customer.surname}, Address: ${customer.address}`;
                customersContainer.appendChild(customerDiv);
            });
        })
        .catch(error => console.error(error));
}

// Function to send GET request to retrieve a customer by ID
function getCustomer() {
    const customerId = document.getElementById('customerId').value;
    fetch(`${baseUrl}/${customerId}`)
        .then(response => response.json())
        .then(data => {
            const customerContainer = document.getElementById('customer');
            customerContainer.innerHTML = '';
            if (data.id) {
                const customerDiv = document.createElement('div');
                customerDiv.textContent = `ID: ${data.id}, Name: ${data.name}, Surname: ${data.surname}, Address: ${data.address}`;
                customerContainer.appendChild(customerDiv);
            } else {
                customerContainer.textContent = 'Customer not found.';
            }
        })
        .catch(error => console.error(error));
}

// Function to send POST request to create a new customer
function createCustomer() {
    const name = document.getElementById('name').value;
    const surname = document.getElementById('surname').value;
    const address = document.getElementById('address').value;

    const customerData = {
        name: name,
        surname: surname,
        address: address
    };

    fetch(baseUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(customerData)
    })
        .then(response => {
            if (response.ok) {
                alert('Customer created successfully.');
            } else {
                throw new Error('Failed to create customer.');
            }
        })
        .catch(error => console.error(error));
}

// Function to send PUT request to update an existing customer
function updateCustomer() {
    const customerId = document.getElementById('customerIdToUpdate').value;
    const name = document.getElementById('nameToUpdate').value;
    const surname = document.getElementById('surnameToUpdate').value;
    const address = document.getElementById('addressToUpdate').value;

    const customerData = {
        id: customerId,
        name: name,
        surname: surname,
        address: address
    };

    fetch(baseUrl, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(customerData)
    })
        .then(response => {
            if (response.ok) {
                alert('Customer updated successfully.');
            } else {
                throw new Error('Failed to update customer.');
            }
        })
        .catch(error => console.error(error));
}

// Function to send DELETE request to delete a customer by ID
function deleteCustomer() {
    const customerId = document.getElementById('customerIdToDelete').value;
    fetch(`${baseUrl}/${customerId}`, {
        method: 'DELETE'
    })
        .then(response => {
            if (response.ok) {
                alert('Customer deleted successfully.');
            } else {
                throw new Error('Failed to delete customer.');
            }
        })
        .catch(error => console.error(error));
}

// Function to send POST request to move balance for a customer
function moveBalance() {
    const customerId = document.getElementById('customerIdToMove').value;
    const amount = document.getElementById('amount').value;

    const moveBalanceData = {
        amount: parseFloat(amount)
    };

    fetch(`${baseUrl}/${customerId}/balance`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(moveBalanceData)
    })
        .then(response => {
            if (response.ok) {
                alert('Balance moved successfully.');
            } else {
                throw new Error('Failed to move balance.');
            }
        })
        .catch(error => console.error(error));
}
