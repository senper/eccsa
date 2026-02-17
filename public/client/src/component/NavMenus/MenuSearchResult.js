import React from "react";
import { Grid } from "@material-ui/core";
import { Link, useLocation } from "react-router-dom";
import getLinkRoute, { search } from "../../services/search";
import Footer from "../Footer/Footer";

export default function MenuSearchResult() {
  const location = useLocation();
  const [loading, setLoading] = React.useState(false);
  const [searchResult, setSearchResult] = React.useState([]);
  let searchKeyValue = 0;
  if (location.state) {
    const { searchKey } = location.state;
    searchKeyValue = searchKey;
  }
  React.useEffect(() => {
    if (searchKeyValue) {
      setLoading(true);
      search(searchKeyValue)
        .then((data) => {
          setSearchResult(data.data.$values);
          setLoading(false);
        })
        .catch((error) => {
          setLoading(false);
        });
    }
  }, [searchKeyValue]);
  return (
    <Grid container spacing={2} className="grid">
      <Grid item xs={12} md={12}>
        <div class="container">
          {" "}
          <h3 className="bg" style={{ marginBottom: "0", marginLeft: "0" }}>
            Search Results
          </h3>
          {loading ? <h3>Loading...</h3> : null}
          {searchResult && searchResult.length > 0 ? (
            <>
              <ul>
                {searchResult.map((row, index) => (
                  <li key={row.id} style={{ marginBottom: "7px" }}>
                    <Link
                      title={row.brief}
                      to={getLinkRoute(
                        row.contentTypeInt,
                        row.contentType,
                        row.contentId
                      )}
                    >
                      {row.title}
                    </Link>
                  </li>
                ))}
              </ul>
            </>
          ) : null}
          {!loading && (!searchResult || searchResult.length <= 0) ? (
            <h3 style={{ marginTop: "20px" }}>NOTHING FOUND</h3>
          ) : null}
        </div>
      </Grid>
    </Grid>
  );
}
