window.addEventListener('load', (e) => {
    const toast = document.getElementById('toast_body');
    if (toast.innerText.trim() != '') $("#notification_toast").toast('show');
    const params = new Proxy(new URLSearchParams(window.location.search), {
        get: (searchParams, prop) => searchParams.get(prop),
    });
    $('#keyword_search').val(params.keyword || '');
    $('#startDate_search').val(params.startDate || '');
    $('#endDate_search').val(params.endDate || '');
});
const findProduct = (e) => {
    let id = e.previousElementSibling.value;
    const idField = document.getElementsByClassName('product-info');
    let result = false;
    for (let i = 0; i < idField.length; i++) {
        if (idField[i].value == id) {
            result = true;
            break;
        }
    }
    if (result) {
        showToast(false, `Sản phẩm bị trùng`);
    } else {
        axios({
            method: 'post',
            url: '/admin/product/find',
            data: {
                id: id
            }
        }).then(res => {
            if (res.data.status) {
                let index = document.getElementById('btnAdd').dataset.i;
                const data = e.parentElement.nextElementSibling;
                e.nextElementSibling.value = id;
                data.innerHTML = `
                        <div class="col-12 d-flex flex-column">
                            <div class="col-12 d-flex">
                                <div class="col-2 border" style="height: 15vh">
                                    <img src="${res.data.data.Image}" alt="" style="width: 100%;height: 100%;object-fit:cover">
                                </div>
                                <div class="col-10">
                                    <div class="ms-3">
                                        <p class="p-0 m-0 fw-bold">${res.data.data.Name}</p>
                                        <div class="row d-flex align-items-center border-bottom col-10">
                                            <label class="form-label col-3 m-0">Số lượng: </label>
                                            <input type="number" min="1" class="col-8 qty" name="ImportBillDetails[${index}].Quantity"
                                                   style="border: none" placeholder="Nhập số lượng sản phẩm">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <button class="btn btn-danger col-4 ms-auto" onclick="deleteLineItem(this)" type="button">Xóa</button>
                        </div>
                `;
            } else {
                showToast(res.data.status, res.data.message);
            }
        })
    }
};

const addLineItem = (e) => {
    const idField = document.getElementsByClassName('product-info');
    if (idField[idField.length - 1].value != '') {
        const fakeIdField = document.getElementsByClassName('id_product');
        const qty = document.getElementsByClassName('qty');
        const data = [];
        for (let i = 0; i < fakeIdField.length; i++) data.push({ id: idField[i].value, qty: qty[i].value, fakeID: fakeIdField[i].value });
        e.dataset.i = parseInt(e.dataset.i) + 1;
        document.getElementById('items').innerHTML += `
                    <hr>
                    <div>
                        <div class="input-group mb-3 col-12">
                            <input type="text" class="form-control id_product" placeholder="Nhập ID sản phẩm" value="">
                            <button class="btn btn-primary" type="button" onclick="findProduct(this)">Kiểm tra</button>
                            <input name="ImportBillDetails[${e.dataset.i}].ProductID" class="d-none product-info" />
                        </div>
                        <div class="col-12 d-flex">

                        </div>
                    </div>
                `;
        for (let i = 0; i < fakeIdField.length - 1; i++) {
            fakeIdField[i].value = data[i].fakeID;
            qty[i].value = data[i].qty;
            idField[i].value = data[i].id;
        }
    }
};

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

let deleteLineItem = (e) => {
    const btnAdd = document.getElementById('btnAdd');
    if (parseInt(btnAdd.dataset.i) > 0) {
        if (e.parentElement.parentElement.parentElement.nextElementSibling)
            e.parentElement.parentElement.parentElement.nextElementSibling.remove();
        if (e.parentElement.parentElement.parentElement)
            e.parentElement.parentElement.parentElement.remove();
        btnAdd.dataset.i = parseInt(btnAdd.dataset.i) - 1;
        let index = parseInt(btnAdd.dataset.i);
        const idField = document.getElementsByClassName('product-info');
        const qty = document.getElementsByClassName('qty');
        console.log(idField);
        console.log(qty);
        for (let i = 0; i <= index; i++) {
            idField[i].name = `ImportBillDetails[${i}].ProductID`;
            qty[i].name = `ImportBillDetails[${i}].Quantity`;
        }
    } else {
        showToast(false, 'Số lượng sản phẩm không được để trống');
    }
};

// Content
let listProduct = document.getElementById('list_product');
let addForm = document.getElementById('add_form');
let action = document.getElementsByClassName('action_product');
for (let i = 0; i < action.length; i++) {
    action[i].addEventListener('click', (e) => {
        for (let j = 0; j < action.length; j++) {
            action[j].classList.toggle('action_product_click');
        }
        listProduct.classList.toggle('d-none');
        addForm.classList.toggle('d-none');
    });
}