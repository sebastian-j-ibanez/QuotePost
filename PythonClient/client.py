import requests
import json
import urllib3
from consolemenu import *
from consolemenu.items import *

urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

DOMAIN = "https://localhost:7154"

def print_quote(quote):
    print("Content:", quote["content"])
    print("Author:", quote["author"])
    print("Likes:", quote["likeCount"])

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

def main():
    menu = ConsoleMenu("Quote Client")
    menu_item_get_all = FunctionItem("Get all quotes", get_all_quotes)
    menu.append_item(menu_item_get_all)
    menu.show()

if __name__ == "__main__":
    main()