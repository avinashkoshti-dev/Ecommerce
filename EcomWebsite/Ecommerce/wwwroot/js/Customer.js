$(() => {
    loadData();  
});

//Load Data function
var loadData = (token) => {
    $.ajax({
        url: "https://localhost:7218/api/Customer/GetAllCustomer",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $('.tbody').html("");
            $.each(result.obj, function (key, item) {
                var template = $("#CustomerTable").find(".trrow").clone().removeClass("trrow").removeClass("d-none");
                $(template).find(".firstname").html(item.firstName);
                $(template).find(".lastname").html(item.lastName);
                $(template).find(".city").html(item.city);
                $(template).find(".country").html(item.country);
                $(template).find(".phone").html(item.phone);
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
    var res = validate();

    if (res) {
        var custObj = {
            Id: $('#ID').val() ? parseInt($('#ID').val()) : 0,
            FirstName: $('#firstName').val(),
            LastName: $('#lastName').val(),
            City: $('#city').val(),
            Country: $('#country').val(),
            Phone: $("#phone").val()
        };
        console.log(custObj)

        $.ajax({
            url: "https://localhost:7218/api/Customer/CreateCustomer",
            data: JSON.stringify(custObj),
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
        url: "https://localhost:7218/api/Customer/GetCustomerById/?id=" + ID,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            if (result.status) {
                $('#ID').val(result.obj.id);
                $('#firstName').val(result.obj.firstName);
                $('#lastName').val(result.obj.lastName);
                $('#city').val(result.obj.city);
                $('#country').val(result.obj.country);
                $("#phone").val(result.obj.phone);
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
            url: "https://localhost:7218/api/Customer/DeleteCustomer/?id=" + ID,
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