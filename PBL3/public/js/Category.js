let action = document.getElementsByClassName('action_product');

// Content 
let listProduct = document.getElementById('list_product');
let addForm = document.getElementById('add_form');

for (let i = 0; i < action.length; i++) {
    action[i].addEventListener('click', (e) => {
        for (let j = 0; j < action.length; j++) {
            action[j].classList.toggle('action_product_click');
        }
        console.log(listProduct);
        listProduct.classList.toggle('d-none');
        addForm.classList.toggle('d-none');
    });
}
let deleteCategory = (e) => {
    let table = document.getElementById('list_category');
    let index = e.dataset.index;
    let id = e.dataset.id;
    axios({
        method: 'post',
        url: '/admin/category/delete',
        data: {
            id
        }
    }).then(res => {
        if (res.data.status) {
            let STT = document.getElementsByClassName('STT');
            for (let i = index; i < STT.length; i++) {
                STT[i].innerText = parseInt(STT[i].innerText) - 1;
                STT[i].dataset.index = parseInt(STT[i].dataset.index) - 1;
            };
            e.parentElement.parentElement.parentElement.remove();
            document.getElementById('total_category').innerText = parseInt(document.getElementById('total_category').innerText) - 1;
        }
        showToast(res.data.status, res.data.message);
    })

}