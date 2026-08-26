from flask import Flask, request, jsonify
import yfinance as yf

app = Flask(__name__)


def search_assets(query):
    """
    Search Yahoo Finance using yfinance.Search and return
    the matching quotes.
    """

    search = yf.Search(
        query=query,
        max_results=12,
        news_count=0,
        lists_count=0,
        include_cb=False,
        include_nav_links=False,
        include_research=False,
        include_cultural_assets=False,
        enable_fuzzy_query=False,
        recommended=12,
        timeout=30,
        raise_errors=True
    )

    results = []

    for quote in search.quotes:
        if quote.get("typeDisp") == "Futures" or quote.get("typeDisp") == "Fund" :
            continue
        
        results.append({
            "symbol": quote.get("symbol"),
            "name": quote.get("shortname"),
            "typeDisp": quote.get("typeDisp"),
            #"quoteType": quote.get("quoteType"),
            "exchange": quote.get("exchange"),
            "exchDisp": quote.get("exchDisp"),
        })

    return results


@app.route("/search", methods=["GET"])
def search():
    """
    Search for financial instruments.

    Example:
        /search?q=apple
        /search?q=AAPL
    """

    query = request.args.get("q", "").strip()

    if not query:
        return jsonify({
            "error": "Missing required query parameter 'q'."
        }), 400

    try:
        results = search_assets(query)

        return jsonify({
            "query": query,
            "count": len(results),
            "results": results
        })

    except Exception as exc:
        return jsonify({
            "error": "Yahoo Finance search failed.",
            "details": str(exc)
        }), 500


@app.route("/health", methods=["GET"])
def health():
    """
    Simple health-check endpoint.
    """
    return jsonify({
        "status": "ok"
    })


if __name__ == "__main__":
    app.run(
        host="127.0.0.1",
        port=7000,
        debug=True
    )