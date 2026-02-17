function validateServiceType() {

    const rateBasis = Number(document.getElementById("rateBasis").value);

    var isNameValid = is_validate_name("nameEng");
    const isNonMembersCurrencyValid = is_validate_Currency("nonMemberServiceCharge");
    if (rateBasis == 3) {
        return isNameValid;
    } else if (rateBasis == 0) {
            rateBasis.style.borderColor = "red";
            rateBasis.title = "Required";
            return false;
    } else if (rateBasis == 5) {
       
        return isNameValid &&  isNonMembersCurrencyValid;
    }
    const isMembersCurrencyValid = is_validate_Currency("memberServiceCharge");
   
    const other = document.getElementById("nonMemberServiceCharge");
    const memberRate = Number(document.getElementById("memberServiceCharge").value);
    const otherRate = Number(other.value);
    if (otherRate < memberRate) {
        other.style.borderColor = "red";
        other.title = "Non member rate cannot be less than member rate";
        return false;
    }
    return isNameValid && isMembersCurrencyValid && isNonMembersCurrencyValid;

}


function validateServiceTypeTask() {
    const isOrderNoValid = is_valid_OrderNo("taskOrderNo");
    const isNameValid = is_validate_name("nameEngTask");
    return isNameValid && isOrderNoValid;
}

function validateServiceTypeRequirement() {

    
    const isOrderNoValid = is_valid_OrderNo("requirementOrderNo");
    const isNameValid = is_validate_name("nameEngRequirement");
    
  
    return isNameValid && isOrderNoValid ;

}
function editServiceType(e) {
    const id = document.getElementById('serviceTypeId');
    const nameEng = document.getElementById('nameEng');
    const nameLocal = document.getElementById('nameLocal');
    const descriptionEng = document.getElementById('descriptionEng');
    const descriptionLocal = document.getElementById('descriptionLocal');


    const memberServiceCharge = document.getElementById('memberServiceCharge');
    const nonMemberServiceCharge = document.getElementById('nonMemberServiceCharge');

    const idVal = $(e).attr('data-id');
    const nameEngVal = $(e).attr('data-nameeng');
    const nameLocVal = $(e).attr('data-nameloc');

    const descrEngVal = $(e).attr('data-descren');
    const descrLocVal = $(e).attr('data-descrlo');

    const membVal = $(e).attr('data-mem');

    const rateVal = $(e).attr('data-rate');
    const otherVal = $(e).attr('data-nonmem');

    const statusVal = $(e).attr('data-status');


    id.value = idVal;
    nameEng.value = nameEngVal;
    nameLocal.value = nameLocVal;
    descriptionEng.innerText = descrEngVal;
    descriptionLocal.innerText = descrLocVal;
    memberServiceCharge.value = membVal;
    nonMemberServiceCharge.value = otherVal;

    $('#settingStatus').val(statusVal).change();
    $('#rateBasis').val(rateVal).change();


}

function getSelectedValue() {
    var selectedValue = document.getElementById("rateBasis").value;

    var others = document.getElementById("nonMemberServiceCharge");
    var members = document.getElementById("memberServiceCharge");


    if (selectedValue == 3) {
        others.disabled = true;
        members.disabled = true;

    }
    else {
        others.disabled = false;
        members.disabled = false;
        others.required = true;
        members.required = true;
    }
}

function onRateChanged() {


    const rateBasis = document.getElementById('rateBasis');
    const val = Number(rateBasis.value);

    const memberId = document.getElementById('memberRate');
    const otherId = document.getElementById('otherRate');

    const flatLabelId = document.getElementById('flatRate');
    const otherLabelId = document.getElementById('otherLabel');
    memberId.style.display = 'none';
    otherId.style.display = 'none';
    flatLabelId.style.display = 'none';
    otherLabelId.style.display = 'none';
    if (val === 5) {
        otherId.style.display = '';
        flatLabelId.style.display = '';
    } else if (val === 4) {
        memberId.style.display = '';
        otherId.style.display = '';
        otherLabelId.style.display = '';
    }
}