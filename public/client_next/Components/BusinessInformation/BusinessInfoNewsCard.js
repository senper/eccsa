import React from 'react';
import {
  Grid,
  CardContent,
  CardMedia,
  CardHeader,
  Typography,
  Card,
} from '@mui/material';
import styles from "../../styles/News.module.css";

export default function BusinessInfoNewsCard({
  news,
  getLocal,
  getLocalByVal,
}) {
  const imgurl = (id, extension) => {
    const idStr = id.toString();
    const name = idStr.substr(idStr.length - 15).replace("-", "");
    return `${process.env.NEXT_PUBLIC_API_URL}/membership/documents/${name}${extension}`;
  };
  return (
    <>
      <Grid item xs={12}>
        <Card>
          <CardHeader
            title={getLocalByVal(news.title, news.titleLocal)}
            subheader={
              <>
                {news.link ? (
                  <a href={news.link} target="_blank" rel="noreferrer">
                    {news.link}
                  </a>
                ) : null}
              </>
            }
          />
          {news && news.imageSetted ? (
                        <CardMedia
                          className={styles.cardMedia}
                          image={imgurl(news.id, news.extension)}
                          style={{backgroundSize: "contain"}}
                          title={getLocalByVal(
                            news.title,
                            news.titleLocal
                          )}
                        />
                      ) : null}
          <CardContent>
            <pre>{getLocalByVal(news.description, news.descriptionLocal)}</pre>
          </CardContent>
        </Card>
      </Grid>
    </>
  );
}
