
let btnMinus = document.getElementById('qtyminus');
let btnPlus = document.getElementById('qtyplus');
console.log(btnPlus)
let quantity = document.getElementById('qty');
btnMinus.addEventListener('click', (e) => {
    let value = quantity.value;
    if (value > 0) quantity.value = parseInt(value) - 1;
});
btnPlus.addEventListener('click', (e) => {
    let value = quantity.value;
    quantity.value = parseInt(value) + 1;
});
