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
            for (let i = index; i < STT.length; i++)
            {
                STT[i].innerText = parseInt(STT[i].innerText) - 1;
            };
            e.parentElement.parentElement.parentElement.remove();
        }
        showToast(res.data.status, res.data.message);
    })

}

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

inputImage.onchange = (e) => {
    image.src = URL.createObjectURL(e.target.files[0]);
}

$('#importForm').on('submit', (e) => {
    e.preventDefault();
    const data = {
        id: $('#product_id_modal').val(),
        quantity: $('#product_quantity_modal').val(),
    };
    axios.post('/admin/product/import', data)
        .then(res => {
            if (res.data.status) {
                const stock = document.getElementsByClassName('product_stock_table');
                let index;
                for (let i = 0; i < stock.length; i++) {
                    if (stock[i].dataset.id == data.id) {
                        index = i;
                        break;
                    }
                }
                console.log(stock);
                console.log(index);
                stock[index].innerText = `${parseInt(stock[index].innerText.trim()) + parseInt(data.quantity.trim())}`;
            }
            showToast(res.data.status, res.data.message);
        })
    $('#product_name_modal').val('');
    $('#product_id_modal').val('');
    $('#product_quantity_modal').val('');
    $('#importProductModal').modal('toggle');
})

$("#submitBtn").click(function (e) {
    const formData = new FormData();
    formData.append('Name', $('#name_product').val());
    formData.append('CategoryID', document.getElementById('CategoryID_product').options[document.getElementById('CategoryID_product').selectedIndex].value);
    formData.append('Price', $('#price_product').val());
    formData.append('Image', inputImage.files[0], $('#inputImage').val());
    formData.append('Description', $('#description_product').val());
    formData.append('Mass', $('#mass_product').val());
    formData.append('Power', $('#power_product').val());
    formData.append('Stock', $('#stock_product').val());
    formData.append('Producer', $('#producer_product').val());
    formData.append('ProducerDate', new Date($('#producerDate_product').val()).toLocaleDateString('en-GB'));
    formData.append('MaintenanceTime', $('#maintenanceTime_product').val());
    e.preventDefault();
    axios.post('/admin/product/add',
        formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
    })
        .then((res) => {
            if (!res.data.status) {
                document.getElementById('notification').innerHTML = `
                        <div class="alert alert-danger" role="alert">
                            ${res.data.detail}
                        </div>`;
            }
            showToast(res.data.status, res.data.message);
        })
});