import requests
import urllib3
from consolemenu import ConsoleMenu
from consolemenu.items import FunctionItem, MenuItem

urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

DOMAIN = "http://localhost:5080"


def print_quote(quote):
    print(quote["content"], "\n")
    print("-", quote["author"], "\n")
    print("Likes:", quote["likeCount"])
    print("------")


def get_all_quotes():
    # Set route and header.
    route = DOMAIN + "/api/quotes"
    header = {
        'Accept': 'application/json'
    }
    # Make request with route and header.
    resp = requests.get(route, headers=header, verify=False)
    # Get response body.
    quotes = {}
    if resp.status_code == 200:
        quotes = resp.json()
    # Print quotes.
    print("---All Quotes---\n")
    for quote in quotes:
        print_quote(quote)
        print()
    input("Press any key to return to main menu...")
    return quotes


def get_random_quote():
    # Set route and header.
    route = DOMAIN + "/api/quotes/random"
    header = {
            'Accept': 'application/json'
    }
    # Make request
    resp = requests.get(route, headers=header, verify=False)
    if resp.status_code == 200:
        print("---Random Quote---\n")
        quote = resp.json()
        print_quote(quote)
    else:
        print("Error:", resp.status_code)
    input("Press any key to return to main menu...")


def add_quote(content, author):
    # Set route and body.
    route = DOMAIN + "/api/quotes"
    body = {
        "Content": content,
        "Author": author
    }
    # Make request.
    resp = requests.post(route, json=body)
    if resp.status_code == 201:
        print("Successfully created quote:")
        quote = resp.json()
        print_quote(quote)
    else:
        print("Error:", resp.status_code)


def add_quote_from_input():
    # Get author and quote content from input.
    content = input("Please enter the quote:")
    author = input("Please enter the author name:") or "Unknown"
    add_quote(content, author)
    input("Press any key to return to the main menu...")


def add_quotes_from_file():
    file_name = input("Enter a file name (default is 'quotes.txt'):") or "quotes.txt"
    file = open(file_name, "r")
    lines = file.readlines()
    # Iterate through lines and add quotes to server.
    for line in lines:
        content, author = line.split("-")
        add_quote(content, author)
    input("Press any key to return to the main menu...")


def main():
    menu = ConsoleMenu("Quote Client")
    menu_item_get_all = FunctionItem("Get all quotes", get_all_quotes)
    menu_item_get_random = FunctionItem("Get random quote", get_random_quote)
    menu_item_add_quote = FunctionItem("Add a quote", add_quote)
    menu_item_add_quotes = FunctionItem("Add quotes from file", add_quotes_from_file)
    menu.append_item(menu_item_get_all)
    menu.append_item(menu_item_get_random)
    menu.append_item(menu_item_add_quote)
    menu.append_item(menu_item_add_quotes)
    menu.show()


if __name__ == "__main__":
    main()
