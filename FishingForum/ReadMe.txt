TODO Category is not removed from list unless page is refreshed : Admin.cshtml.cs
TODO Return to created post : CreateThread.cshtml.cs









// WORKAROUND Remove the ModelState entry for Category to bypass its validation : EditCategory.cshtml.cs
// WORKAROUND Remove the ModelState entry for Category to bypass its validation : EditSubCategory.cshtml.cs


// Should not have named DAL.UserManager that name since it conflicts with UserManager from identity

// Add limit to thread title length
// Add ability to remove thread
// If there are more than a single space in a row the rest are removed which ruins post formating. Fix to allow additional spaces.
// Add hyperlink recognition in post