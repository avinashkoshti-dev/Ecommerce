$(() => {
    loadData();
    GetSupplier();

    $("#myModal").find("#btnAdd").on("click",(e)=> {
        Add();
    });
});

//Load Data function
var loadData = (token) => {
    $.ajax({
        url: "https://localhost:7218/api/Product/GetAllProduct",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $('.tbody').html("");
            $.each(result.obj, function (key, item) {
                console.log(item.isDiscontinued)
                var template = $("#ProductTable").find(".trrow").clone().removeClass("trrow").removeClass("d-none");
                $(template).find(".pname").html(item.productName);
                $(template).find(".sname").html(item.supplierName);
                $(template).find(".uprice").html(item.unitPrice);
                $(template).find(".package").html(item.package);
                $(template).find(".IsDiscontinued").html(item.isDiscontinued.toString());
                $(template).find(".btnEdit").attr("data-id", item.id).on("click", (e) => {
                    getbyID($(e.currentTarget).attr('data-id'));
                });
                $(template).find(".btnDelete").attr("data-id", item.id).on("click", (e) => {
                    Delele($(e.currentTarget).attr('data-id'))
                });
                $('.tbody').append(template);
            });
            
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//Add Data Function
var Add = () => {
    //var res = validate();

    if (true) {
        var productObj = {
            Id: $('#ID').val() ? parseInt($('#ID').val()) : 0,
            ProductName: $('#pName').val(),
            SupplierId: parseInt($('#sName').val()),
            UnitPrice: parseInt($('#UnitPrice').val()),
            Package: $('#package').val(),
            IsDiscontinued: $("#IsDiscontinued").prop('checked'),
            
        };
        console.log(productObj)

        $.ajax({
            url: "https://localhost:7218/api/Product/CreateProduct",
            data: JSON.stringify(productObj),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                console.log(result)
                if (result.status) {
                    reset();
                    loadData();
                    toastr.success(result.message);
                    $('#myModal').modal('hide');
                }
                else {
                    toastr.error(result.message);
                }
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
}

var GetSupplier = () => {
    console.log("calling")
    $.ajax({
        url: "https://localhost:7218/api/Product/GetAllSupplier",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var template = $("#myModal");
            console.log($(template).find("#sName"))
            $(template).find("#sName").html("");
            $.each(result.obj, function (key, item) {
                console.log(item)
                $(template).find("#sName").append(' <option value="' + item.id + '">"' + item.companyName + '"</option>');
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//Valdidation using jquery
var validate = () => {
    var isValid = true;
    if ($('#firstName').val().trim() == "") {
        $('#firstName').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#firstName').css('border-color', 'lightgrey');
    }
    if ($('#lastName').val().trim() == "") {
        $('#lastName').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#lastName').css('border-color', 'lightgrey');
    }

    if ($('#city').val().trim() == "") {
        $('#city').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#city').css('border-color', 'lightgrey');
    }
    if ($('#country').val().trim() == "") {
        $('#country').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#country').css('border-color', 'lightgrey');
    }
    if ($('#phone').val().trim() == "") {
        $('#phone').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#phone').css('border-color', 'lightgrey');
    }
    return isValid;
}

//Edit Data function
var getbyID = (ID) => {
    $.ajax({
        url: "https://localhost:7218/api/Product/GetProductById/?id=" + ID,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            if (result.status) {
                console.log(result.obj)
                $('#ID').val(result.obj.id);
                $('#pName').val(result.obj.productName);
                $('#sName').val(result.obj.supplierId);
                $('#UnitPrice').val(result.obj.unitPrice);
                $('#package').val(result.obj.package);
                $("#IsDiscontinued").prop("checked", result.obj.isDiscontinued);

                $('#myModal').modal('show');
                $("#myModal").find("#btnAdd").off().on("click", (e) => {
                    Add();
                })
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//function for deleting employee's record
function Delele(ID) {
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        $.ajax({
            url: "https://localhost:7218/api/Product/DeleteProduct/?id=" + ID,
            type: "DELETE",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                console.log(result)
                if (result.status) {
                    loadData();
                    toastr.success(result.message);
                }
                else {
                    toastr.error(result.message);
                }
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
}

//Function for clearing the textboxes
function reset() {
    $('#ID').val("");
    $("#firstName").val("");
    $("#lastName").val("");
    $("#city").val("");
    $("#country").val("");
    $("#phone").val("");
}