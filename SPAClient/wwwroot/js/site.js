// Form table rows.
let _quoteTableRows = $('#quoteTableRows');

// Add/edit quote form elements.
let _quoteFormTitle = $('#quoteFormTitle');
let _quoteFormContent = $('#quoteFormContent');
let _quoteFormAuthor = $('#quoteFormAuthor');
let _quoteFormTags = $('#quoteFormTags');
_quoteFormTags.hide();
let _quoteFormTagsLabel = $('#quoteFormTagLabel');
_quoteFormTagsLabel.hide();
let _quoteTagList = $('#tagList');
let _quoteFormSubmit = $('#quoteFormSubmit');
_quoteFormSubmit.bind("click", function() {
    addQuote();
}); 
let _quoteFormReset = $('#quoteFormReset');
_quoteFormReset.click(function() {
    resetQuoteForm();
});

// Load the quote table.
async function loadQuoteTable() {
    const resp = await fetch('http://localhost:5080/api/quotes', {
        method: 'GET',
        mode: 'cors',
        headers: {
            'Accept': 'application/json',
        },
    });
    
    // Get quotes from response json.
    let quotes = await resp.json();

    _quoteTableRows.empty();
    
    // Init table row from quote JSON and append to table.
    quotes.forEach(quote => {
        // Init row and content/author columns.
        let row = $('<tr>');
        let contentCell = $('<td>').addClass('align-middle').text(quote.content);
        let authorCell = $('<td>').addClass('align-middle').text(quote.author);
        
        // Setup tag cell and append each associated tag.
        let tagCell = $('<td>').addClass('justify-content-center align-items-center align-middle').text('');
        let tags = quote.quoteTags;
        tags.forEach(tag => {
            let newTag = $('<span>').addClass('badge rounded-pill bg-primary align-middle').text(tag.tag.tagName).data('tagid', tag.tagId);
            tagCell.append(newTag);
        });
        
        // Setup likes cell with like and dislike buttons.
        let likesCell = $('<td>').addClass('justify-content-center align-items-center align-middle').text('');
        let likeCount = $('<span>').addClass('badge text-primary justify-content-center align-middle me-2 ms-4').text(quote.likeCount);
        let likeButton = $('<button>').addClass('btn btn-sm justify-content-center align-middle')
            .html('&#x2795;')
            .bind("click",function() {
            likeQuote(quote.quoteId);
        });
        let dislikeButton = $('<button>').addClass('btn btn-sm justify-content-center align-middle')
            .html('&#x2796;')
            .bind("click",function() {
            dislikeQuote(quote.quoteId);
        });
        likesCell.append(likeCount, likeButton, dislikeButton);
        
        // Setup 'Actions' column with edit and delete buttons.
        let actionCell = $('<td>').addClass('justify-content-center align-items-center').text('');
        let editButton = $('<button>').addClass('btn btn-sm btn-primary m-1 align-middle')
            .text('Edit')
            .bind("click", function() {
                showEditForm(quote);
            });
        let deleteButton = $('<button>').addClass('btn btn-sm btn-primary align-middle')
            .text('Delete')
            .bind("click", function() {
            deleteQuote(quote.quoteId)
        });
        actionCell.append(editButton, deleteButton);
        
        row.append(contentCell, authorCell, tagCell, likesCell, actionCell);
        _quoteTableRows.append(row);
    });
    
    await setFormTags();
}

// Make 'like' request and reload table.
async function likeQuote(quoteId) {
    await fetch(`http://localhost:5080/api/quotes/${quoteId}/like`, {
        method: 'POST',
        mode: 'cors',
    });
    
    await loadQuoteTable();
}

// Make 'dislike' request and reload table.
async function dislikeQuote(quoteId) {
    await fetch(`http://localhost:5080/api/quotes/${quoteId}/dislike`, {
        method: 'POST',
        mode: 'cors',
    });

    await loadQuoteTable();
}

// Function to reset form when toggle form is clicked
async function resetQuoteForm() {
    _quoteFormTitle.text('Add Quote');
    _quoteFormContent.val('');
    _quoteFormAuthor.val('');
    _quoteFormTags.val('');
    _quoteFormTagsLabel.hide();
    _quoteFormTags.hide();
    _quoteFormSubmit.unbind("click");
    _quoteFormSubmit.bind("click", function () {
        addQuote();
    });
}

// Show edit form filled with quote information.
async function showEditForm(quote) {
    // Setup form.
    _quoteFormTitle.text('Edit Quote');
    
    // Populate form fields with quote information.
    _quoteFormContent.val(quote.content);
    _quoteFormAuthor.val(quote.author);
    let quoteTags = quote.quoteTags;
    if (quoteTags.length > 0) {
        _quoteFormTags.val(quoteTags[0].tag.tagName);
    }
    
    // Show tag section.
    _quoteFormTags.show();
    _quoteFormTagsLabel.show();
    
    // Unbind/bind submission button with editQuote function.
    _quoteFormSubmit.unbind("click");
    _quoteFormSubmit.bind("click", await function() {
        editQuote(quote.quoteId);
    });
}

// Add a new quote.
async function addQuote() {
    if (_quoteFormContent.val().trim() === '') {
        alert("Quote field is empty!");
    }
    else {
        let quote = {
            'content': _quoteFormContent.val(),
            'author': _quoteFormAuthor.val(),
        };

        console.log();
        console.log(quote);
        let quoteBody = JSON.stringify(quote);
        let quoteId = -1;
        console.log(quoteBody);
        fetch('http://localhost:5080/api/quotes', {
            method: 'POST',
            mode: 'cors',
            headers: {
                'Content-Type': 'application/json',
            },
            body: quoteBody
        })
            .then(response => {
                if (response.ok) {
                    console.log(response);
                }
            })
            .catch(error => {
                console.log(error);
            });
    }
}

// PATCH a quote based on quoteId.
async function editQuote(quoteId) {
    console.log(quoteId);
    // Get quote from form fields.
    let quote = {
        "Content": _quoteFormContent.val(),
        "Author": _quoteFormAuthor.val()
    };

    // PATCH quote at ID.
    fetch(`http://localhost:5080/api/quotes/${quoteId}`, {
        method: 'PATCH',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(quote),
    })
        .then(response => {
            console.log(response);
            if (response.ok) {
                loadQuoteTable();
            }
        })
        .catch(error => {
            console.log("Error PATCH quote:", error);
        });

    if (_quoteFormTags.val()) {
        getTagId()
            .then(tagId => {
                fetch(`http://localhost:5080/api/quotes/${quoteId}/tag/${tagId}`, {
                    method: 'POST',
                }).then(response => {
                    console.log(response);
                }).catch(error => {
                    console.log("Error POST quote and tag:", error);
                });
            })
            .catch(error => {
                console.log("Error getting tag Id", error);
            });
    }
}

// Get tagId from _quoteFromTags
async function getTagId() {
    // Check if tag input is an existing tag.
    let resp = await fetch('http://localhost:5080/api/quotes/tags', {
        method: 'GET',
        mode: 'cors',
    });

    let tagInput = _quoteFormTags.val();
    tagInput = tagInput.toString();
    let tags = await resp.json();
    
    let tagId = -1;
    tags.forEach(tag => {
        // console.log(tag);
        // alert(tag);
        if (tag.tagName == tagInput) {
            tagId = tag.tagId;
        }
    });
    
    if (tagId == -1) {
        let tagBody = {
            'tagName': tagInput
        };

        // Create tag if it does not exist.
        fetch('http://localhost:5080/api/quotes/tag', {
            method: 'POST',
            mode: 'cors',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(tagBody),
        }).then(response => {
            let newTagJSON = response.json();
            tagId = newTagJSON.tagId;
        }).catch(error => {
            alert(error);
            tagId = -1;
        });
    }
    
    return tagId;
}

// DELETE a quote based on quoteId.
async function deleteQuote(quoteId) {
    await fetch(`http://localhost:5080/api/quotes/${quoteId}`, {
        method: 'DELETE',
        mode: 'cors',
    });
    
    loadQuoteTable();
}

// Get all tags, append them to form datalist.
async function setFormTags() {
    let resp = await fetch(`http://localhost:5080/api/quotes/tags`, {
        method: 'GET',
        mode: 'cors',
    });
    
    _quoteTagList.empty();
    
    let tags = await resp.json();
    tags.forEach(tag => {
        console.log(tag);
        let newTagOption = $('<option>').val(tag.tagName).data('tagid', tag.tagId);
        _quoteTagList.append(newTagOption);
    });
}

loadQuoteTable();