

//nav header
var navHeader = $('.nav-header')
navHeader.append(`
<div class="brand-logo">
<a href="index.html">

    <h4>Logo</h4>

    <span class="brand-title">
   
    </span>
</a>
</div>
`)


var unread = ()=>{
    var unread = $(".msg-unread")
//   console.log(unread.length)
    var newMsgnode = document.querySelectorAll(".new-msg")
    newMsgnode.forEach(element => {
        element.innerText = unread.length
    });
    unread = $(".notification-unread");
    var newNotificationNode = document.querySelectorAll(".new-notification")
    newNotificationNode.forEach(element => {
        element.innerText = unread.length
    });
}
unread()

/// add more features
var featureBtn = $("#add-more-feature")
var featureNode = $(".features")
featureBtn.click(()=>{
    featureNode.append(`
    <div class="form-row">
    <div class="form-group col-md-4">
       
        <input type="text" name="feature" class="form-control" required>
    </div>
    <div class="form-group col-md-4">
       
        <input type="text" name="feature" class="form-control" required >
    </div>
    <div class="form-group col-md-3">
        <input type="text" name="feature" class="form-control" required >
  
    </div>
    <div class="form-group col-md-1 d-flex justify-content-center align-items-center">
    <button type="button" class="btn btn-danger" onclick="deleteThisFeaturerow(this)">X</button>
    

</div>

</div>
    `)
})

//feature Delete

function deleteThisFeaturerow(_this){

$(_this).parent().siblings().parent().remove()
}
