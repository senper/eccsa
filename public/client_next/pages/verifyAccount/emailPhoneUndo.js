import React, { useState } from "react";
import { getLocalByKey } from "../api/menu";
import { Grid } from "@mui/material";
import { useRouter } from "next/router";
import Link from "next/link";

export default function EmailPhoneUndo({ translations }) {
  const location = useRouter();
  const { locale } = location;

  const getLocal = (key) => {
    return getLocalByKey(key, translations, locale);
  };
  const getLocalByVal = (eng, local) => {
    return locale == "en" || !local ? eng : local;
  };

  return (
    <>
      <Grid container style={{paddingTop: "15px"}}>
        <Grid item xs={1} md={3}></Grid>
        <Grid item xs={10} md={6}>
            <h3 style={{ fontSize: "20px", marginTop: "0", fontWeight: "bolder" }}>
              {getLocal("Email/Mobile change undone Successfully! Click ")}
              <Link href="/login">Login</Link>
              {" to go to login page."}
            </h3>
        </Grid>
        <Grid item xs={1} md={3}></Grid>
      </Grid>
      <br />
      <br />
    </> 
  );
}
