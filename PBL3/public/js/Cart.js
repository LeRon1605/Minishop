const minusProduct = (e) => {
    let input = e.parentNode.nextElementSibling;
    let value = input.value;
    if (value > 1) {
        input.value = parseInt(value) - 1;
    }
}


const plusProduct = (e) => {
    let input = e.parentNode.previousElementSibling;
    let value = input.value;
    input.value = parseInt(value) + 1;
}