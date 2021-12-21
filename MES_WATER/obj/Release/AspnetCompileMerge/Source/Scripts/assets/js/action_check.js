


// 檢查輸入是否超出範圍，超出範圍為True
function Check_Length(targetId, maxlength) {
    if ($("#" + targetId).val().length > maxlength) {
        //清空input
        $("#" + targetId).val("");
        return true;
    }
    return false;
}

/* check_format(value, type) return boolean // type: Number, Date, Mail*/