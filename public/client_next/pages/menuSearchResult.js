import { Container, Grid } from "@mui/material";
import Link from "next/link";
import { useRouter } from "next/router";
import React from "react";
import getLinkRoute, { search } from "../api/search";
import { getLocalByKey } from "./api/menu";

export default function MenuSearchResult({ translations }) {
  const location = useRouter();
  const { locale } = location;
  const [loading, setLoading] = React.useState(false);
  const [searchResult, setSearchResult] = React.useState([]);
  let searchKeyValue = 0;
  if (location.query) {
    const { searchKey } = location.query;
    searchKeyValue = searchKey;
  }
  const getLocal = (key) => {
    return getLocalByKey(key, translations, locale);
  };
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
        {" "}
        <h3 className="bg" style={{ marginBottom: "0", marginLeft: "0", marginTop: "0px" }}>
          {getLocal("Search Results")}
        </h3>
        {loading ? <h3>{getLocal("Loading")}...</h3> : null}
        {searchResult && searchResult.length > 0 ? (
          <>
            <ul>
              {searchResult.map((row, index) => (
                <li key={row.id} style={{ marginBottom: "7px" }}>
                  <Link
                    title={row.brief}
                    href={getLinkRoute(
                      row.contentTypeInt,
                      row.contentType,
                      row.contentId
                    )}
                    legacyBehavior>
                    {row.title}
                  </Link>
                </li>
              ))}
            </ul>
          </>
        ) : null}
        {!loading && (!searchResult || searchResult.length <= 0) ? (
          <h3 style={{ marginTop: "20px" }}>{getLocal("NOTHING FOUND")}</h3>
        ) : null}
      </Grid>
    </Grid>
  );
}
