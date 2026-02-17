
function isPassword(inputId) {
    const input = document.getElementById(inputId);
    input.style.borderColor = "black";
    input.title = "";
    const pattern = /(?=!@#$%^&.*-_+\d)(?=.*[a-z])(?=.*[A-Z]).{8,}/;
    if (input.value == null || input.value.trim() == "" || pattern.test(input.value)) {
        input.style.borderColor = "red";
        input.title = "Invalid Password";
        return false;
    }
    return true;
}

function is_validate_name(inputId) {
    const name = document.getElementById(inputId);
    name.style.borderColor = "black";
    name.title = "";
    const fname = trim(name.value);
    const regName = /^[a-zA-Z0-9._\s]*$/;
    var illegalChars = /\W/; // allow letters, numbers, and underscores
    if (fname === "" || fname.length <= 2 || fname.length > 100 || !regName.test(fname)) {

        name.title = "cannot be less than two characters and only   alphabets, numbers . or underscore are allowed";
        name.style.borderColor = "red";
        return false;
    }
    return true;
}

function is_validate_email(inputId) {
    const email = document.getElementById(inputId);
    email.style.borderColor = "black";
    email.title = "";
    const emailValue = trim(email.value);
    const re = /^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;
    if (!re.test(emailValue)) {
        email.style.borderColor = "red";
        email.title = "Invalid email";
        return false;
    }
    return true;

}

function validatePhoneNumber(input_str) {
    var re = /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/im;

    return re.test(input_str);
}

function is_validate_mobile(inputId) {
    const mobile = document.getElementById(inputId);
    mobile.style.borderColor = "";
    mobile.title = "";
    let phone = trim(mobile.value);

    if (phone == '') {
        return true;
    }
    if (!validatePhoneNumber(phone)) {
        const phoneError = document.getElementById("phone_error");
        phoneError.style.display = "block";
        return false;
    } else {
        return true;
    }

}
function is_validate_local_phone(inputId) {
    const mobile = document.getElementById(inputId);
    mobile.style.borderColor = "";
    mobile.title = "";
    let phone = trim(mobile.value);
    const phoneFormat = /^\+251\d{9}$/;
    phone = phone.replace(/[\(\)\.\-\ ]/g, '');
    //checking if phone number is in 09xxxxxxxx format and replacing the first digit with +251
    if (phone.length == 10) {
        phone.indexOf('0') == 0 ? phone = phone.replace('0', '+251') : phone;
    }

    if (phone === "" && ((phone.length < 10)) && ((phone.length > 13))) {

        phone.style.borderColor = "red";
        phone.title = "Invalid mobile";
        return false;
    }
    if (phone.match(phoneFormat)) {
        return true;
    }
    mobile.style.borderColor = "red";
    mobile.title = "Invalid phone";
    return false;
}
function is_validate_phone(inputId) {
   
    return true;
}



function is_validate_Currency(inputId) {
    const currency = document.getElementById(inputId);
    currency.style.borderColor = "";
    currency.title = "";

    const currencyValue = trim(currency.value);
    var regex = /^[1-9]\d*(((,\d{3}){1})?(\.\d{0,2})?)$/;
    if (currencyValue.match(regex)) {
        return true;
    }
    currency.style.borderColor = "red";
    currency.title = "Invalid currency";
    return false;



}

function is_valid_Decimal(inputId) {
    const regEx = /^[0-9]*\.?[0-9]*$/;
    const orderNo = document.getElementById(inputId);
    orderNo.style.borderColor = "";
    orderNo.title = "";
    const orderNoValue = trim(orderNo.value);

    if (orderNoValue.match(regEx)) {
        return true;
    }
    orderNo.style.borderColor = "red";
    orderNo.title = "Invalid OrderNo";
    return false;

}

function is_valid_Integer(inputId) {
    const regEx = /^[0-9]*$/;
    const orderNo = document.getElementById(inputId);
    orderNo.style.borderColor = "";
    orderNo.title = "";
    const orderNoValue = trim(orderNo.value);

    if (orderNoValue.match(regEx)) {
        return true;
    }
    orderNo.style.borderColor = "red";
    orderNo.title = "Invalid OrderNo";
    return false;

}

function is_valid_OrderNo(inputId) {
    const regEx = /^[0-9]*\.?[0-9]*$/;
    const orderNo = document.getElementById(inputId);
    orderNo.style.borderColor = "";
    orderNo.title = "";
    const orderNoValue = trim(orderNo.value);

    if (orderNoValue.match(regEx)) {
        return true;
    }
    orderNo.style.borderColor = "red";
    orderNo.title = "Invalid OrderNo";
    return false;

}


function is_valid_TIN(inputId) {
    const regEx = /^[0-9]*$/;
    const orderNo = document.getElementById(inputId);
    orderNo.style.borderColor = "";
    orderNo.title = "";
    const orderNoValue = trim(orderNo.value);

    if (orderNoValue.match(regEx) && orderNoValue.length == 10) {
        return true;
    }
    orderNo.style.borderColor = "red";
    orderNo.title = "Invalid TIN";
    return false;

}

function trim(s) {
    return s.replace(/^\s+|\s+$/, '');
}

function is_valid_url(inputId) {
    const Url = document.getElementById(inputId);
    const str = Url.value;
    var pattern = new RegExp('^(https?:\\/\\/)?' + // protocol
        '((([a-z\\d]([a-z\\d-]*[a-z\\d])*)\\.)+[a-z]{2,}|' + // domain name
        '((\\d{1,3}\\.){3}\\d{1,3}))' + // OR ip (v4) address
        '(\\:\\d+)?(\\/[-a-z\\d%_.~+]*)*' + // port and path
        '(\\?[;&a-z\\d%_.~+=-]*)?' + // query string
        '(\\#[-a-z\\d_]*)?$', 'i'); // fragment locator
    if (!!pattern.test(str)) {
        return true;
    }
    Url.style.borderColor = "red";
    Url.title = "Invalid Url";
    return false;
}

function is_valid_date(inputId) {
    const dateInput = document.getElementById(inputId);
    const dateText = dateInput.value;
    const isValid = validatedate(dateText, "mm/dd/yyyy");
    if (isValid) return true;
    dateInput.style.borderColor = "red";
    dateInput.title = "Invalid date";
    return false;
}
function is_valid_date_YMD(inputId) {
    const dateInput = document.getElementById(inputId);
    const dateText = dateInput.value;
    const isValid = validatedate2(dateText, "yyyy-mm-dd");
    if (isValid) return true;
    dateInput.style.borderColor = "red";
    dateInput.title = "Invalid date";
    return false;
}
function validatedate(inputText, DateFormat) {
    // format dd/mm/yyyy or in any order of (dd or mm or yyyy) you can write dd or mm or yyyy in first or second or third position ... or can be slash"/" or dot"." or dash"-" in the dates formats
    var invalid = "";
    var dt = "";
    var mn = "";
    var yr = "";
    var k;
    var delm = DateFormat.includes("/") ? "/" : (DateFormat.includes("-") ? "-" : (DateFormat.includes(".") ? "." : ""));
    var f1 = inputText.split(delm);
    var f2 = DateFormat.split(delm);
    for (k = 0; k <= 2; k++) {
        dt = dt + (f2[parseInt(k)] == "dd" ? f1[parseInt(k)] : "");
        mn = mn + (f2[parseInt(k)] == "mm" ? f1[parseInt(k)] : "");
        yr = yr + (f2[parseInt(k)] == "yyyy" ? f1[parseInt(k)] : "");
    }
    var mn_days = "0-31-" + (yr % 4 == 0 ? 29 : 28) + "-31-30-31-30-31-31-30-31-30-31";
    var days = mn_days.split("-");
    if (f1.length != 3 || mn.length > 2 || dt.length > 2 || yr.length != 4 || !(parseInt(mn) >= 1 && parseInt(mn) <= 12) || !(parseInt(yr) >= parseInt(1900) && parseInt(yr) <= parseInt(2100)) || !(parseInt(dt) >= 1 && parseInt(dt) <= parseInt(days[parseInt(mn)]))) {
        return false;
    }
    return true;
}