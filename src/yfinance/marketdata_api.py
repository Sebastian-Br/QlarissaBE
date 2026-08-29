from datetime import datetime

from flask import Flask, jsonify, request
import yfinance as yf

app = Flask(__name__)


@app.get("/security")
def get_security_data():
    symbol = request.args.get("symbol")
    start_date = request.args.get("startdate")

    # Validate required parameters.
    if not symbol:
        return jsonify({
            "error": "Missing required parameter: symbol"
        }), 400

    if not start_date:
        return jsonify({
            "error": "Missing required parameter: start-date"
        }), 400

    # Validate date format.
    try:
        datetime.strptime(start_date, "%Y-%m-%d")
    except ValueError:
        return jsonify({
            "error": "start-date must be in YYYY-MM-DD format"
        }), 400

    try:
        ticker = yf.Ticker(symbol)

        # ------------------------------------------------------------------
        # Ticker information
        # ------------------------------------------------------------------
        # ticker.info returns a dictionary containing information such as:
        # longName, sector, industry, marketCap, currency, exchange, etc.
        info = ticker.info

        # ------------------------------------------------------------------
        # Historical daily prices
        # ------------------------------------------------------------------
        history = ticker.history(
            start=start_date,
            interval="1d",
            auto_adjust=False
        )

        if not history.empty:
            # Make the DataFrame index explicit so it can be serialized.
            history = history.reset_index()
            history = history.drop(columns=["Adj Close", "Volume"], errors="ignore")
            # Convert pandas Timestamp objects to JSON-compatible strings.
            history["Date"] = history["Date"].dt.strftime("%Y-%m-%d")

            # Convert NaN/NaT to None.
            history = history.where(history.notna(), None)

            history_data = history.to_dict(orient="records")
        else:
            history_data = []

        # ------------------------------------------------------------------
        # Response
        # ------------------------------------------------------------------
        return jsonify({
            "info": info,
            "history": history_data
        })

    except Exception as exc:
        return jsonify({
            "error": str(exc)
        }), 500


if __name__ == "__main__":
    app.run(
        host="0.0.0.0",
        port=7001,
        debug=True
    )