// Button action group
let action = document.getElementsByClassName('action_product');

// Content 
let listProduct = document.getElementById('list_product');
let addForm = document.getElementById('add_form');

// Image
let inputImage = document.getElementById('inputImage');
let image = document.getElementById('image');

let showToast = (status, message) => {
    const toastBody = document.getElementById('toast_body');
    if (status) {
        if (toastBody.classList.contains('bg-danger')) toastBody.classList.remove('bg-danger');
        if (!toastBody.classList.contains('bg-success')) toastBody.classList.add('bg-success');
    } else {
        if (!toastBody.classList.contains('bg-danger')) toastBody.classList.add('bg-danger');
        if (toastBody.classList.contains('bg-success')) toastBody.classList.remove('bg-success');
    }
    toastBody.innerText = message;
    $("#notification_toast").toast('show');
}

let transferData = (e) => {
    let name = e.dataset.name;
    let id = e.dataset.id;

    $('#product_name_modal').val(name);
    document.getElementById('product_id_modal').value = id;
};

let deleteProduct = (e) =>
{
    let table = document.getElementById('list_product');
    let index = e.dataset.index;
    let id = e.dataset.id;
    axios({
        method: 'post',
        url: '/admin/product/delete',
        data: {
            id
        }
    }).then(res => {
        if (res.data.status) {
            let STT = document.getElementsByClassName('STT');
            let btnDelete = document.getElementsByClassName('btnDelete');
            for (let i = index; i < STT.length; i++)
            {
                STT[i].innerText = parseInt(STT[i].innerText) - 1;
                btnDelete[i].dataset.index = parseInt(btnDelete[i].dataset.index) - 1;
            };
            e.parentElement.parentElement.parentElement.remove();
            document.getElementById('total_product').innerText = parseInt(document.getElementById('total_product').innerText) - 1;
        }
        showToast(res.data.status, res.data.message);
    })

}

for (let i = 0; i < action.length; i++) {
    action[i].addEventListener('click', (e) => {
        for (let j = 0; j < action.length; j++) {
            action[j].classList.toggle('action_product_click');
        }
        listProduct.classList.toggle('d-none');
        addForm.classList.toggle('d-none');
    });
}

inputImage.onchange = (e) => {
    image.src = URL.createObjectURL(e.target.files[0]);
}

$('#importForm').on('submit', (e) => {
    e.preventDefault();
    const data = {
        productID: $('#product_id_modal').val(),
        quantity: $('#product_quantity_modal').val(),
        totalprice: $('#product_total_modal').val(),
    };
    axios.post('/admin/product/import', data)
        .then(res => {
            if (res.data.status) {
                const stock = document.getElementsByClassName('product_stock_table');
                let index;
                for (let i = 0; i < stock.length; i++) {
                    if (stock[i].dataset.id == data.productID) {
                        index = i;
                        break;
                    }
                }
                stock[index].innerText = `${parseInt(stock[index].innerText.trim()) + parseInt(data.quantity.trim())}`;
                $('#product_name_modal').val('');
                $('#product_id_modal').val('');
                $('#product_quantity_modal').val('');
                $('#importProductModal').modal('toggle');
                showToast(res.data.status, res.data.message);
            } else {
                const notification = document.querySelector("#notification");
                notification.innerHTML = `<div class="alert alert-danger col-12 p-2" role="alert">`
                    + res.data.detail +
                `</div>`;
            }
        })
})

window.addEventListener('load', (e) => {
    const params = new Proxy(new URLSearchParams(window.location.search), {
        get: (searchParams, prop) => searchParams.get(prop),
    });
    const toast = document.getElementById('toast_body');
    $('#keyword_search').val(params.keyword || '');
    $('#state_search').val(params.state || '');
    $('#category_search').val(params.CategoryID || 'All');
    $('#minValue_search').val(params.minValue || '');
    $('#maxValue_search').val(params.maxValue || '');
    if (toast.innerText.trim() != '') $("#notification_toast").toast('show');
})

$('#submitBtn').click(e => {
    let ProductDate = document.getElementById('producerDate_product');
    let validationMessage = document.getElementById('validation-message');
    const now = new Date();
    now.setHours(0, 0, 0, 0);
    if (new Date(ProductDate.value).getTime() > now.getTime()) {
        if (validationMessage.classList.contains('d-none')) validationMessage.classList.remove('d-none');
        e.preventDefault();
    } else {
        if (!validationMessage.classList.contains('d-none')) validationMessage.classList.add('d-none');
    }
});