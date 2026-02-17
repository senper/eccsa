function validateMediaAgency() {
    const isValidUrl = is_valid_url("idurl");
    const isValidEmail = is_validate_email("idemail");
    const isValidMobile = is_validate_mobile("idmobile");
    const isValidRepName = is_validate_name("repNameId");
    const isValidContactName = is_validate_name("idpername");
    const isValidName = is_validate_name("nameId");
    
    return isValidUrl &&
        isValidEmail &&
        isValidMobile &&
        isValidRepName &&
        isValidName &&
        isValidContactName;

}
function valideCampaign() {
    const isValidStartDate = is_valid_date_YMD("startDateId");
    const isValidEndDate = is_valid_date_YMD("endDateId");
    if (isValidEndDate && isValidStartDate) {
        const stDate = document.getElementById("startDateId")
        const enDate = document.getElementById("endDateId")
        const startDate = new Date(stDate.value);
        const endDate = new Date(enDate.value);
        if (endDate.getTime() < startDate.getTime()) {
            stDate.style.borderColor = "red";
            stDate.title = "Start Date should be smaller than end date";
            enDate.style.borderColor = "red";
            enDate.title = "End Date >= Start Date";
            return false;
        }
        return true;
    }
    return false;
}

function valideCoverage() {
    const isValidStartDate = is_valid_date_YMD("startDateId");
    const isValidEndDate = is_valid_date_YMD("endDateId");
    if (isValidEndDate && isValidStartDate) {
        const stDate = document.getElementById("startDateId")
        const enDate = document.getElementById("endDateId")
        const startDate = new Date(stDate.value);
        const endDate = new Date(enDate.value);
        if (endDate.getTime() < startDate.getTime()) {
            stDate.style.borderColor = "red";
            stDate.title = "Start Date should be smaller than end date";
            enDate.style.borderColor = "red";
            enDate.title = "End Date >= Start Date";
            return false;
        }
        return true;
    }
    return false;
}

