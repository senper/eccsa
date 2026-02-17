export function isPassword(inputId) {
  const input = document.getElementById(inputId);
  const pattern = /(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}/;
  if (input.value == null || pattern.test(input.value)) {
    return false;
  }
  return true;
}

export function is_validate_name(value) {
  const fname = trim(value);
  const regName = /^[a-zA-Z\s]*$/;
  var illegalChars = /\W/; // allow letters, numbers, and underscores
  if (
    fname === "" ||
    fname.length <= 2 ||
    fname.length > 100 ||
    !regName.test(fname)
  ) {
    return false;
  }
  return true;
}

export function is_validate_email(value) {
  const emailValue = trim(value);
  const re =
    /^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;
  if (!re.test(emailValue)) {
    return false;
  }
  return true;
}

export function is_validate_mobile(value) {
  let phone = trim(value);
  const phoneFormat = /^\+2519\d{8}$/;
  phone = phone.replace(/[\(\)\.\-\ ]/g, "");
  //checking if phone number is in 09xxxxxxxx format and replacing the first digit with +251
  if (phone.length == 10) {
    phone = phone.indexOf("0") == 0 ? phone.replace("0", "+251") : phone;
  }

  if (phone === "" && phone.length < 10 && phone.length > 13) {
    return false;
  }
  if (phone.match(phoneFormat)) {
    return true;
  }
  return false;
}

export function is_validate_Currency(value) {
  const currencyValue = trim(value);
  var regex = /^[1-9]\d*(((,\d{3}){1})?(\.\d{0,2})?)$/;
  if (currencyValue.match(regex)) {
    return true;
  }
  return false;
}

export function is_valid_Decimal(value) {
  const regEx = /^[0-9]*\.?[0-9]*$/;
  const orderNoValue = trim(value);

  if (orderNoValue.match(regEx)) {
    return true;
  }
  return false;
}

export function is_valid_Integer(value) {
  const regEx = /^[0-9]*$/;
  const orderNoValue = trim(value);

  if (orderNoValue.match(regEx)) {
    return true;
  }
  return false;
}

export function is_valid_OrderNo(value) {
  const regEx = /^[0-9]*\.?[0-9]*$/;
  const orderNoValue = trim(value);

  if (orderNoValue.match(regEx)) {
    return true;
  }
  return false;
}

export function is_valid_TIN(value) {
  const regEx = /^[0-9]*$/;
  const orderNoValue = trim(value);

  if (orderNoValue.match(regEx) && orderNoValue.length == 10) {
    return true;
  }
  return false;
}

export function trim(s) {
  return s.replace(/^\s+|\s+$/, "");
}

export function getNameFromId(id) {
  const idStr = id.toString();
  const name = idStr.substr(idStr.length - 15).replace("-", "");
  return name;
}
