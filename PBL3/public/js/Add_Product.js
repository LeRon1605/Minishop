// Button action group
let action = document.getElementsByClassName('action_product');

// Content 
let listProduct = document.getElementById('list_product');
let addForm = document.getElementById('add_form');

// Image
let inputImage = document.getElementById('inputImage');
let image = document.getElementById('image');

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

$("#submitBtn").click(function (e) {
    const data =
    {
        Name: $('#name_product').val(),
        Image: $('#inputImage').val(),
        CategoryID: document.getElementById('CategoryID_product').options[document.getElementById('CategoryID_product').selectedIndex].value,
        Price: $('#price_product').val(),
        Description: $('#description_product').val(),
        Mass: $('#mass_product').val(),
        Power: $('#power_product').val(),
        Stock: $('#stock_product').val(),
        Producer: $('#producer_product').val(),
        ProducerDate: new Date($('#producerDate_product').val()),
        MaintenanceTime: $('#maintenanceTime_product').val()
    };
    const formData = new FormData();
    formData.append('Name', data.Name);
    formData.append('CategoryID', data.CategoryID);
    formData.append('Price', data.Price);
    formData.append('Image', inputImage.files[0], data.Image);
    formData.append('Description', data.Description);
    formData.append('Mass', data.Mass);
    formData.append('Power', data.Power);
    formData.append('Stock', data.Stock);
    formData.append('Producer', data.Producer);
    formData.append('ProducerDate', new Date(data.ProducerDate).toLocaleDateString('en-GB'));
    formData.append('MaintenanceTime', data.MaintenanceTime);
    e.preventDefault();
    axios.post('/admin/product/add',
        formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
    })
        .then((res) => {
            const toastBody = document.getElementById('toast_body');
            if (res.data.status) {
                if (toastBody.classList.contains('bg-danger')) toastBody.classList.remove('bg-danger');
                if (!toastBody.classList.contains('bg-success')) toastBody.classList.add('bg-success');
            } else {
                if (!toastBody.classList.contains('bg-danger')) toastBody.classList.add('bg-danger');
                if (toastBody.classList.contains('bg-success')) toastBody.classList.remove('bg-success');
                document.getElementById('notification').innerHTML = `
                        <div class="alert alert-danger" role="alert">
                            ${res.data.detail}
                        </div>`;
            }
            toastBody.innerText = res.data.message;
            $("#notification_toast").toast('show');
        })
});
