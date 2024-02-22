function belal(e) {
    alert('Come');
    var secondEditor = $("#DropSubCategory").dxSelectBox("instance");
    secondEditor.getDataSource().filter(['CategoryId', '=', e.value]);
    secondEditor.getDataSource().load();
}

