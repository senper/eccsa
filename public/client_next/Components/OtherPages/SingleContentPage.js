import { Paper } from "@mui/material";
import OtherPagesContentCard from "./OtherPagesContentCard";

export default function SingleContentPage({
  contents,
  getLocal,
  getLocalByVal,
  loadingContentsList,
}) {
  if (loadingContentsList)
    return (
      <strong>
        <br />
        {getLocal("Loading")}...
      </strong>
    );

  return contents && contents.length > 0 ? (
    <Paper style={{padding: "6px"}}>
      <OtherPagesContentCard
        content={contents[0]}
        getLocal={getLocal}
        getLocalByVal={getLocalByVal}
      />
    </Paper>
  ) : null;
}
