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
/*
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
    let formData = new FormData();
    formData.append('Name', $('#name_product').val());
    formData.append('Image', inputImage.files[0], 'random.png');
    formData.append('CategoryID', document.getElementById('CategoryID_product').options[document.getElementById('CategoryID_product').selectedIndex].value);
    e.preventDefault();
    $.ajax({
        type: "POST",
        url: '/product/add',
        data: formData,
        dataType: "json",
        enctype: 'multipart/form-data',
        contentType: "application/json; charset=utf-8",
                success: (data) => {
                    const notification = document.querySelector("#notification");
                    if (data.status) {
                        notification.innerHTML = `<div class="alert alert-success col-sm-12 col-md-10 p-2" role="alert">`
                            + data.message +
                            `</div>`;
                    } else {
                        notification.innerHTML = `<div class="alert alert-danger col-sm-12 col-md-10 p-2" role="alert">`
                            + data.message +
                            `</div>`;
                    }
        },
        error: function () {

        }
       });
       return false;
});
*/