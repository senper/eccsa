function validateService() {

    const modalBody = document.getElementById("serviceRequestBody");
    const emails = modalBody.getElementsByClassName("email");
    const integers = modalBody.getElementsByClassName("number");
    const decimals = modalBody.getElementsByClassName("decimal");
    const texts = modalBody.getElementsByClassName("text");
    const mobiles = modalBody.getElementsByClassName("mobile");
    const tins = modalBody.getElementsByClassName("tin");
    let isValid = true;
    for (let i = 0; i < emails.length; i++) {
        const email = emails[i].id;
        const v1 = is_validate_email(email);
        isValid = isValid && v1
    }
    for (let i = 0; i < mobiles.length; i++) {
        const mobile = mobiles[i].id;
        const v2 = is_validate_mobile(mobile);
        isValid = isValid && v2;
    }

    for (let i = 0; i < integers.length; i++) {
        const integer = integers[i].id;
        const v3 = is_valid_Integer(integer);
        isValid = isValid && v3;
    }
    for (let i = 0; i < decimals.length; i++) {
        const decimal = decimals[i].id;
        const v4 = is_valid_Decimal(decimal);
        isValid = isValid && v4;
    }
    for (let i = 0; i < tins.length; i++) {
        const tin = tins[i].id;
        const v5 = is_valid_TIN(tin);
        isValid = isValid && v5
    }
    return isValid;

}

