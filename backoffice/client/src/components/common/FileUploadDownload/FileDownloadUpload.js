/* eslint-disable react/prop-types */
import React from "react";
import { Link } from "react-router-dom";
import Button from "components/vender/CustomButtons/Button.js";
import { makeStyles } from "@material-ui/core/styles";

const useUploadStyles = makeStyles((theme) => ({
  root: {
    "& > *": {
      margin: theme.spacing(1),
    },
  },
  input: {
    display: "none",
  },
}));

export default function FileDownloadUpload({ setFile }) {
  const dofileUpload = React.useRef(null);
  const dofileDownload = React.useRef(null);
  const [att_filename, setAtt_Filename] = React.useState("");
  const [fileDownloadUrl, setFileDownloadUrl] = React.useState("");

  const uploadClasses = useUploadStyles();

  const openFile = (evt) => {
    let status = []; // Status output
    const fileObj = evt.target.files[0];
    setFile(fileObj);
    const fileDownloadUrl = URL.createObjectURL(fileObj);
    setFileDownloadUrl(fileDownloadUrl);
    setAtt_Filename(fileObj.name);
  };

  return (
    <>
      <div className={uploadClasses.root}>
        <input
          accept="image/*"
          className={uploadClasses.input}
          id="contained-button-file"
          multiple
          type="file"
          onChange={(evt) => openFile(evt)}
          ref={dofileUpload}
        />
        <label htmlFor="contained-button-file">
          <Button variant="contained" color="info" component="span" size="sm">
            Upload File
          </Button>
        </label>
        {fileDownloadUrl.length > 0 ? (
          <Link
            download={att_filename}
            to={fileDownloadUrl}
            ref={dofileDownload}
          >
            {att_filename}
          </Link>
        ) : null}
      </div>
    </>
  );
}
