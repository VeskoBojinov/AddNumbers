// Base URL of the Numbers API
const apiUrl = 'http://localhost:5233/api/numbers';

/**
 * Fetches the current list of numbers from the API and updates the UI.
 * - Renders each number as a styled "number-box"
 * - Updates the count
 * - Resets the sum display
 */
async function renderNumbers() {
    const res = await fetch(apiUrl);
    const data = await res.json();

    const numberList = document.getElementById('numberList');
    numberList.innerHTML = ''; // Clear existing list

    // Render each number in its own styled box
    data.numbers.forEach(num => {
        const box = document.createElement('div');
        box.className = 'number-box';
        box.textContent = num;
        numberList.appendChild(box);
    });

    // Display the number count
    document.getElementById('count').textContent = data.count;

    // Reset the sum display (until summed)
    document.getElementById('sum').textContent = 'Not summed';
}

/**
 * Sends a DELETE request to clear all numbers from the API,
 * then re-renders the number list.
 */
async function clearNumbers() {
    await fetch(apiUrl, { method: 'DELETE' });
    renderNumbers();
}

/**
 * Sends a POST request to add a random number to the list,
 * then re-renders the number list.
 */
async function addNumber() {
    await fetch(apiUrl, { method: 'POST' });
    renderNumbers();
}

/**
 * Sends a GET request to retrieve the sum of all numbers
 * and displays it in the UI.
 */
async function sumNumbers() {
    const res = await fetch(`${apiUrl}/sum`);
    const data = await res.json();

    // Display the sum
    document.getElementById('sum').textContent = data.sum;
}

// Initial page load: fetch and display numbers
renderNumbers();