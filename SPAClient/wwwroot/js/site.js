let _quoteTableRows = $('#quoteTableRows');

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
            let newTag = $('<span>').addClass('badge rounded-pill bg-primary align-middle').text(tag.tag.tagName);
            tagCell.append(newTag);
        });
        
        // Setup likes cell with like and dislike buttons.
        let likesCell = $('<td>').addClass('justify-content-center align-items-center align-middle').text('');
        let likeCount = $('<span>').addClass('badge text-primary justify-content-center align-middle me-2 ms-4').text(quote.likeCount);
        let likeButton = $('<button>').addClass('btn btn-sm justify-content-center align-middle')
            .html('&#x2795;')
            .click(function() {
            likeQuote(quote.quoteId);
        });
        let dislikeButton = $('<button>').addClass('btn btn-sm justify-content-center align-middle')
            .html('&#x2796;')
            .click(function() {
            dislikeQuote(quote.quoteId);
        });
        likesCell.append(likeCount, likeButton, dislikeButton);
        
        // Setup 'Actions' column with edit and delete buttons.
        let actionCell = $('<td>').addClass('justify-content-center align-items-center').text('');
        let editButton = $('<button>').addClass('btn btn-sm btn-primary m-1 align-middlei')
            .text('Edit')
            .click(function() {
                showEditForm(quote);
            });
        let deleteButton = $('<button>').addClass('btn btn-sm btn-primary align-middle')
            .text('Delete')
            .click(function() {
            deleteQuote(quote.quoteId)
        });
        actionCell.append(editButton, deleteButton);
        
        row.append(contentCell, authorCell, tagCell, likesCell, actionCell);
        _quoteTableRows.append(row);
    });
    
    setFormTags();
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

// Show edit form filled with quote information.
// Call editQuote function when submitted.
async function showEditForm(quote) {
    // Setup form.
    
}

// PATCH a quote based on quoteId.
async function editQuote(quoteId) {
    let resp = await fetch(`http://localhost:5080/api/quotes/${quoteId}`, {
        method: 'PATCH',
        mode: 'cors',
    });
    
    // If response code is 201:
    // Reset form.
    // Load quote table.
    if (resp.status === 201) {
        await loadQuoteTable();
    }
}

// DELETE a quote based on quoteId.
async function deleteQuote(quoteId) {
    await fetch(`http://localhost:5080/api/quotes/${quoteId}`, {
        method: 'DELETE',
        mode: 'cors',
    });
    
    await loadQuoteTable();
}

// Get all tags, append them to form datalist.
async function setFormTags() {
    let _tagList = $('#tagList');
    
    let resp = await fetch(`http://localhost:5080/api/quotes/tags`, {
        method: 'GET',
        mode: 'cors',
    });
    
    let tags = resp.json();
    tags.forEach(tag => {
        let newTagOption = $('<option>').value(tag.tagName);
        _tagList.append(newTagOption);
    });
}

loadQuoteTable();